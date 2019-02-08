<template>
  <list-table title="Resources"
      :endpoint="endpoint">
  </list-table>
</template>

<script>
  import List from '../shared/list-table'

  export default {
    components: {
      'list-table': List
    },

    data() {
      return {
        endpoint: '/api/Resources'
      }
    }
  }
</script>

<style>
</style>

