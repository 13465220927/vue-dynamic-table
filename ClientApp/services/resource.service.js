import Vue from 'vue'
import axios from 'axios'

const client = axios.create({
  baseURL: '/api/Resources',
  json: true
})

export default {

  async execute(method, resource, data) {
    
    return client({
      method,
      url: resource,
      data
    }).then(req => {
      return req
    })
  },

  async getAll() {
    let response = await this.execute('get', '/');

    if (response.data == 0) {
      return response.data;
    }
    
    var languages = [];
    var headers = [];
    headers.push("key");

    response.data.map((row) => {
      languages.push(row.languageCode);
    });

    languages = Array.from(new Set(languages));
    
    Array.prototype.push.apply(headers, languages.sort((a, b) => a.localeCompare(b)));

    var resultsObj = {
      results: response.data.reduce((res, { key, languageCode, translation }) => {

        if (!res[key]) {
          res[key] = Object.assign({}, ...languages.map(id => ({ [id]: {} })));
        }

        res[key][languageCode] = translation;
        res[key]["key"] = key;
        return res;
      }, {})
    };

    return { items: resultsObj.results, headers: headers };
  },
  create(data) {
    return this.execute('post', '/', data)
  },
  update(id, data) {
    return this.execute('put', `/${id}`, data)
  },
  delete(id) {
    return this.execute('delete', `/${id}`);
  }
}
