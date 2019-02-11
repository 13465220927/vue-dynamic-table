<template>
  <div>
    <h1>{{ title }}</h1>

    <section v-if="errored">
      <p>We're sorry, we're not able to retrieve this information at the moment, please try back later</p>
    </section>

    <div v-if="loading" class="text-center">
      <p><em>Loading...</em></p>
      <h1><icon icon="spinner" pulse /></h1>
    </div>

    <div v-if="noItems">
      <p><em>The {{ title }} table is empty.</em></p>
    </div>

    <template v-if="items">
      <table class="container">
        <thead class="bg-dark text-white">
          <tr>
            <th contentEditable
                ref="thRef"
                v-on:keyup.enter.self.stop.prevent="enterKeyHeader(index)"
                v-for="(header, index) in headers">{{ header }}</th>
            <th>
              <button class="btn btn-outline-light" v-on:click="addColumn()">
                +
              </button>
            </th>
          </tr>
        </thead>
        <tbody>
          <tr ref="trRef" v-for="(item, key, indexRow) in items" :key="indexRow">
            <td contentEditable
                ref="tdRef"
                v-for="(header, indexCol) in headers" :key="indexCol"
                v-if='show'
                v-on:keyup.enter="enterKeyRow(indexRow, indexCol, key)"
                @keydown.tab="onTab(indexRow, indexCol, key)">
              {{ item[header] }}
            </td>
            <td>
              <button class="btn btn-danger" v-on:click="deleteKey(key)">
                -
              </button>
            </td>
          </tr>
          <tr>
            <td class="bg-light">
              <button class="btn btn-success" v-on:click="addKey()"> + </button>
            </td>
          </tr>
        </tbody>
      </table>

      <v-dialog :resizable="true" />
    </template>
  </div>
</template>
<script>
  import api from '../../services/resource.service';

  export default {
    props: {
      title: {
        type: String,
        required: true
      }
    },
    data() {
      return {
        items: null,
        loading: true,
        errored: false,
        noItems: false,
        show: true,
        headers: []
      }
    },
    mounted() {
      document.addEventListener("keyup", this.nextItem);
    },
    methods: {
      async loadPage() {
        try {

          let response = await api.getAll();

          if (response.length == 0) {
            this.noItems = true;
            return;
          }

          this.items = response.items;
          this.headers = response.headers;
          
        } catch (err) {
          console.log(err);
          this.errored = true;
        } finally {
          this.loading = false
        }
      },

      addColumn() {
        this.headers.push("");
        this.$nextTick(() => {
          const count = this.$refs.thRef.length;
          this.$refs.thRef[count - 1].focus();
        })
      },
      enterKeyHeader(index) {
               
        Object.entries(this.items).map((row) => {
          row[1][this.$refs.thRef[index].innerText.trim()] = '';
        });
        
        //hack to force re render
        this.show = false;
        this.$nextTick(() => {
          this.show = true;

          this.$nextTick(() => {
            this.headers[index] = this.$refs.thRef[index].innerText.trim();

            this.$nextTick(() => {
              const count = this.$refs.trRef[0].cells.length;
              this.$refs.trRef[0].cells[count - 2].focus();
            })
          });
        });
      },

      async enterKeyRow(indexRow, indexCol, key) {
        
        const indexKey = (indexRow + 1) * this.headers.length - this.headers.length;
        //key = this.$refs.tdRef[indexKey].innerText;

        console.log(key);

        if (indexCol == 0) {
          return;
        }

        const index = (indexRow + 1) * this.headers.length + indexCol - this.headers.length;
        
        if (this.$refs.tdRef[index].innerText.trim() != "") {
          await this.createKey(key.trim(), this.headers[indexCol], this.$refs.tdRef[index].innerText.trim());
          this.items[key][this.headers[indexCol]] = this.$refs.tdRef[index].innerText.trim();
        }

        console.log(this.items);
        
        if (this.$refs.tdRef[index + this.headers.length] != undefined) {
          this.$refs.tdRef[index + this.headers.length].focus();
        }
      },

      async createKey(key, languageCode, translation) {
        await api.create({
          "languageCode": languageCode,
          "translation": translation,
          "key": key
        })
          .then(function (response) {
            console.log(response);
          })
          .catch(function (error) {
            console.log(error);
          });
      },

      deleteKey(key) {
        this.$modal.show('dialog', {
          title: 'Alert',
          text: `Are you sure you want to delete the key ${key} ?`,
          buttons: [
            {
              title: 'Cancel'
            },
            {
              title: 'Accept',
              default: true, // Will be triggered by default if 'Enter' pressed.
              handler: () => {
                this.deleteItem(key);
                this.$modal.hide('dialog');
              }
            }
          ]
        })
      },

      addKey() {
        this.items['new_key'] = {};
        this.items['new_key']['new_language'] = "";
        this.items['new_key']['key'] = "";

        this.show = false;
        this.$nextTick(() => {
          this.show = true;
          this.$nextTick(() => {
            const index = (this.$refs.trRef.length) * this.headers.length - this.headers.length;
            this.$refs.tdRef[index].focus();
          });
        });

        console.log(this.items);
      },

      deleteItem(key) {
        try {
          api.delete(key.trim());
          delete this.items[key];

          this.show = false;
          this.$nextTick(() => {
            this.show = true;
          })
        } catch (err) {
          console.log(err);
        }
      },

      async onTab(indexRow, indexCol, key) {
        
        if (indexCol == 0) {
          return;
        }

        const index = (indexRow + 1) * this.headers.length + indexCol - this.headers.length;
        const indexKey = (indexRow + 1) * this.headers.length - this.headers.length;

        try {
          var response = await api.update(this.$refs.tdRef[indexKey].innerText.trim(), {
            "languageCode": this.headers[indexCol],
            "translation": this.$refs.tdRef[index].innerText.trim(),
            "key": this.$refs.tdRef[indexKey].innerText.trim()
          });
          console.log(response);
        } catch (err) {
          console.log(err);
        }
        
        if (key == "new_key") {
          delete this.items[key];

          this.items[this.$refs.tdRef[indexKey].innerText] = Object.assign({}, ...this.headers.map(id => ({ [id]: {} })));;
          this.items[this.$refs.tdRef[indexKey].innerText][this.headers[indexCol]] = this.$refs.tdRef[index].innerText.trim();
          this.items[this.$refs.tdRef[indexKey].innerText]['key'] = this.$refs.tdRef[indexKey].innerText.trim();
          
        }
        else {
          this.items[key][this.headers[indexCol]] = this.$refs.tdRef[index].innerText.trim();
        }
        
        console.log(this.items);
      }
    },

    async created() {
      this.loadPage()
    }

    
    
  }</script>

<style>

  .container {
    font-family: Tahoma;
    font-size: 0.9em;
  }

  table {
    border-collapse: collapse;
    overflow: hidden;
    box-shadow: 0 0 20px rgba(0, 0, 0, 0.3);
  }

  td {
    padding: 15px;
  }

  th {
    text-align: left;
    padding: 15px;
  }

  thead th {
    background-color: #55608f;
  }

  tbody td {
    position: relative;
  }

  tbody tr:hover {
    background-color: rgba(128, 128, 128, 0.17);
  }

  tbody td:hover:before {
    content: "";
    position: absolute;
    left: 0;
    right: 0;
    top: -9999px;
    bottom: -9999px;
    background-color: rgba(128, 128, 128, 0.17);
    z-index: -1;
  }


</style>
