<template>
  <v-row>
    <v-col cols="2">
      <v-sheet rounded="lg">
        <v-list :rounded="true">
          <v-list-item>Everything</v-list-item>
          <v-list-item v-for="n in 5" :key="n" link>
            {{ 'List item ' + n }}
          </v-list-item>

          <v-divider class="my-2"></v-divider>

          <v-list-item
            color="grey-lighten-4"
            title="Refresh"
            link
          ></v-list-item>
        </v-list>
      </v-sheet>
    </v-col>

    <v-col style="border: 1px solid black">
      <v-sheet min-height="70vh" rounded="lg" style="border: 5px solid red">
        <h2>Hello world</h2>
      </v-sheet>
    </v-col>
  </v-row>
</template>

<script>
export default {
  name: 'IndexPage',
};
</script>
