<template>
  <v-row>
    <v-col class="d-flex justify-center">
      <v-sheet
        outlined
        elevation="10"
        width="90vw"
        min-height="70vh"
        rounded="lg"
        class="pa-14 mt-16"
      >
        <div class="text-h4 mb-6">Servers</div>
        <v-data-table :headers="headers" height="50vh" :items="servers">
          <template v-slot:top>
            <v-toolbar flat>
              <v-spacer></v-spacer>
              <v-dialog v-model="dialog" max-width="500px">
                <template v-slot:activator="{ on, attrs }">
                  <v-btn
                    color="primary"
                    dark
                    class="mb-2"
                    v-bind="attrs"
                    v-on="on"
                  >
                    Add new Server
                  </v-btn>
                </template>
                <v-card>
                  <v-card-title>
                    <span class="text-h5"> New Server </span>
                  </v-card-title>

                  <v-card-text>
                    <v-container>
                      <div>
                        <v-text-field
                          v-model="editedItem.name"
                          label="Server name"
                        ></v-text-field>
                        <v-text-field
                          v-model="editedItem.calories"
                          label="Calories"
                        ></v-text-field>
                        <v-text-field
                          v-model="editedItem.fat"
                          label="Fat (g)"
                        ></v-text-field>
                        <v-text-field
                          v-model="editedItem.carbs"
                          label="Carbs (g)"
                        ></v-text-field>
                        <v-text-field
                          v-model="editedItem.protein"
                          label="Protein (g)"
                        ></v-text-field>
                      </div>
                    </v-container>
                  </v-card-text>

                  <v-card-actions>
                    <v-spacer></v-spacer>
                    <v-btn color="blue darken-1" text @click="close">
                      Cancel
                    </v-btn>
                    <v-btn color="blue darken-1" text @click="save">
                      Save
                    </v-btn>
                  </v-card-actions>
                </v-card>
              </v-dialog>
              <v-dialog v-model="dialogDelete" max-width="500px">
                <v-card>
                  <v-card-title class="text-h5"
                    >Are you sure you want to delete this item?</v-card-title
                  >
                  <v-card-actions>
                    <v-spacer></v-spacer>
                    <v-btn color="blue darken-1" text @click="closeDelete"
                      >Cancel</v-btn
                    >
                    <v-btn color="blue darken-1" text @click="deleteItemConfirm"
                      >OK</v-btn
                    >
                    <v-spacer></v-spacer>
                  </v-card-actions>
                </v-card>
              </v-dialog>
            </v-toolbar>
          </template>
          <template v-slot:item="{ item, index }">
            <tr @click="showServer(item.id)" class="clickable__row">
              <td>{{ item.name }}</td>
              <td>{{ item.date }}</td>
              <td>{{ item.edition_date }}</td>
              <td>{{ applications[index] }}</td>
              <td>{{ tasks[index] }}</td>
              <td>
                <v-icon small class="mr-2" @click.stop="editItem(item)">
                  mdi-pencil
                </v-icon>
                <v-icon small @click.stop="deleteItem(item)">
                  mdi-delete
                </v-icon>
              </td>
            </tr>
          </template>
          <template v-slot:no-data>
            <v-btn color="primary" @click="initialize"> Reset </v-btn>
          </template>
        </v-data-table>
      </v-sheet>
    </v-col>
  </v-row>
</template>

<script>
export default {
  data() {
    return {
      dialog: false,
      dialogDelete: false,
      name: 'Servers Page',
      headers: [
        { text: 'Name', value: 'name' },
        { text: 'Creation date', value: 'date' },
        { text: 'Edition date', value: 'edition_date' },
        { text: 'Applications' },
        { text: 'Tasks' },
        { text: 'Actions', value: 'actions', sortable: false },
      ],
      desserts: [],
      editedIndex: -1,
      editedItem: {
        name: '',
        calories: 0,
        fat: 0,
        carbs: 0,
        protein: 0,
      },
      defaultItem: {
        name: '',
        calories: 0,
        fat: 0,
        carbs: 0,
        protein: 0,
      },
    };
  },
  computed: {
    servers() {
      return this.$store.getters.getServers;
    },
    formTitle() {
      return this.editedIndex === -1 ? 'New Item' : 'Edit Item';
    },
    applications() {
      return this.servers.map((server) => {
        return this.$store.getters.getApps.filter(
          (app) => app.serverId == server.id
        ).length;
      });
    },
    tasks() {
      return this.servers.map((server) => {
        return this.$store.getters.getTasks.filter(
          (task) => task.serverId == server.id
        ).length;
      });
    },
  },
  watch: {
    dialog(val) {
      val || this.close();
    },
    dialogDelete(val) {
      val || this.closeDelete();
    },
  },
  methods: {
    showServer(id) {
      this.$router.push('/details/servers/' + id);
    },
    editItem(item) {
      this.editedIndex = this.desserts.indexOf(item);
      this.editedItem = Object.assign({}, item);
      this.dialog = true;
    },

    deleteItem(item) {
      this.editedIndex = this.desserts.indexOf(item);
      this.editedItem = Object.assign({}, item);
      this.dialogDelete = true;
    },

    deleteItemConfirm() {
      this.desserts.splice(this.editedIndex, 1);
      this.closeDelete();
    },

    close() {
      this.dialog = false;
      this.$nextTick(() => {
        this.editedItem = Object.assign({}, this.defaultItem);
        this.editedIndex = -1;
      });
    },

    closeDelete() {
      this.dialogDelete = false;
      this.$nextTick(() => {
        this.editedItem = Object.assign({}, this.defaultItem);
        this.editedIndex = -1;
      });
    },

    save() {
      if (this.editedIndex > -1) {
        Object.assign(this.desserts[this.editedIndex], this.editedItem);
      } else {
        this.desserts.push(this.editedItem);
      }
      this.close();
    },
  },
};
</script>
