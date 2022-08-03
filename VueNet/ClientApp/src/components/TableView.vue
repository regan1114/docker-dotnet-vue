<template>
    <v-table>
      <thead>
      <tr>
        <th v-for="header in columnKeys" :key="header.value" class="text-left">
          {{header.text}}
        </th>
      </tr>
    </thead>
    <tbody>
      <tr v-for="item in items"
        :key="item">
        <td v-for="header in columnKeys" :key="header.value">
          <div v-if="header.value === 'datetime'">
            {{dateToString(header,item)}}
          </div>
          <div v-else>
            {{item[header.value] }}
          </div>
        </td>
      </tr>
    </tbody>
    </v-table>
</template>

<script>
import moment from "moment";
  export default {
    props:{
      columnKeys: Array,
      items: Array,
    },
    data () {
      return {
      }
    },
    computed:{
      
    },
    methods:{
      dateToString(header, item){
        const date = item[header.value];
        return moment(date).format("yyyy-MM-DD HH:mm:ss");
      }
    }
  }
</script>