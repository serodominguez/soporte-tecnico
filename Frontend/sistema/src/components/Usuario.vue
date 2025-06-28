<template>
  <v-layout align-start>
    <v-flex>
      <v-toolbar flat color="white">
        <v-toolbar-title>Usuarios</v-toolbar-title>
        <v-divider class="mx-2" inset vertical></v-divider>
        <v-spacer></v-spacer>
        <v-text-field
          class="text-xs-center"
          v-model="search"
          append-icon="search"
          label="Búsqueda"
          single-line
          hide-details
        ></v-text-field>
        <v-spacer></v-spacer>
        <v-dialog v-model="dialog" persistent max-width="500px">
          <v-btn
            slot="activator"
            color="primary"
            dark
            class="mb-2"
            @click="seleccionarPersonal()"
            >Nuevo</v-btn
          >
          <v-card>
            <v-card-title>
              <span class="headline">{{ formTitle }}</span>
            </v-card-title>
            <v-form ref="form">
              <v-container grid-list-md>
                <v-layout wrap>
                  <v-flex v-if="editedIndex != 1" xs12 sm12 md6 lg6 xl6>
                    <v-autocomplete

                      v-model="idpersonal"
                      :items="personales"
                      :rules="[rules.requerido]"
                      label="Personal"
                      no-data-text="No hay datos disponibles"
                    >
                    </v-autocomplete>
                  </v-flex>
                  <v-flex xs12 sm6 md6>
                    <v-text-field
                      v-model="usuario"
                      :rules="[rules.requerido]"
                      @keyup="uppercase"
                      label="Usuario"
                    ></v-text-field>
                  </v-flex>
                  <v-flex xs12 sm6 md6>
                    <v-select
                      v-model="idrol"
                      :items="roles"
                      :rules="[rules.requerido]"
                      label="Rol"
                    >
                    </v-select>
                  </v-flex>
                  <v-flex xs12 sm6 md6>
                    <v-text-field
                      type="password"
                      v-model="password"
                      :rules="[rules.requerido]"
                      label="Contraseña"
                    ></v-text-field>
                  </v-flex>
                </v-layout>
              </v-container>
            </v-form>
            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn color="green darken-1" flat @click.native="guardar"
                >Guardar</v-btn
              >
              <v-btn color="red darken-4" flat @click.native="cerrar"
                >Cancelar</v-btn
              >
            </v-card-actions>
          </v-card>
        </v-dialog>
        <v-dialog v-model="adModal" max-width="290px">
          <v-card>
            <v-card-title class="headline" v-if="adAccion == 1"
              >Activar Item?</v-card-title
            >
            <v-card-title class="headline" v-if="adAccion == 2"
              >Desactivar Item?</v-card-title
            >
            <v-card-text>
              Estás a punto de
              <span v-if="adAccion == 1">Activar</span>
              <span v-if="adAccion == 2">Desactivar</span>
              el ítem {{ adUsuario }}
            </v-card-text>
            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn
                color="green darken-1"
                flat="flat"
                @click="activardesactivarCerrar"
                >Cancelar</v-btn
              >
              <v-btn
                v-if="adAccion == 1"
                color="orange darken-4"
                flat="flat"
                @click="activar"
                >Activar</v-btn
              >
              <v-btn
                v-if="adAccion == 2"
                color="orange darken-4"
                flat="flat"
                @click="desactivar"
                >Desactivar</v-btn
              >
            </v-card-actions>
          </v-card>
        </v-dialog>
      </v-toolbar>
      <v-data-table
        :headers="headers"
        :items="usuarios"
        :search="search"
        :rows-per-page-text="paginas"
        class="elevation-1"
      >
        <template slot="items" slot-scope="props">
          <td>{{ props.item.personal }}</td>
          <td>{{ props.item.rol }}</td>
          <td>{{ props.item.usuario }}</td>
          <td>
            <div v-if="props.item.estado == 'Activo'">
              <span class="green--text">{{ props.item.estado }}</span>
            </div>
            <div v-else>
              <span class="red--text">{{ props.item.estado }}</span>
            </div>
          </td>
          <td>
            <v-icon small class="mr-2" @click="editar(props.item)">edit</v-icon>
            <template v-if="props.item.estado == 'Activo'">
              <v-icon small @click="activardesactivarMostrar(2, props.item)"
                >block</v-icon
              >
            </template>
            <template v-else>
              <v-icon small @click="activardesactivarMostrar(1, props.item)"
                >check</v-icon
              >
            </template>
          </td>
        </template>
        <template slot="no-data">
          <v-btn color="error" @click="listar">Resetear</v-btn>
        </template>
        <template slot="no-results">
          <v-alert :value="true" color="error" icon="warning">
            Tu búsqueda de "{{ search }}" no encontro resultados.
          </v-alert>
        </template>
      </v-data-table>
    </v-flex>
  </v-layout>
</template>
<script>
import axios from "axios";
export default {
  data() {
    return {
      usuarios: [],
      dialog: false,
      headers: [
        { text: "Personal", value: "personal" },
        { text: "Rol", value: "rol" },
        { text: "Usuario", value: "usuario", sortable: false },
        { text: "Estado", value: "estado", sortable: false },
        { text: "Opciones", value: "opciones", sortable: false },
      ],
      paginas: "Areas por Página",
      search: "",
      editedIndex: -1,
      id: "",
      idrol: "",
      idpersonal: "",
      roles: [],
      personales: [],
      usuario: "",
      password: "",
      actualizarPassword: false,
      anteriorPassword: "",
      adModal: 0,
      adAccion: 0,
      adUsuario: "",
      adId: "",
      error: null,
      rules: {
        requerido: (value) => !!value || "Requerido!",
      },
    };
  },
  computed: {
    formTitle() {
      return this.editedIndex === -1 ? "Nuevo usuario" : "Actualizar usuario";
    },
  },

  watch: {
    dialog(val) {
      val || this.cerrar();
    },
  },

  created() {
    this.listar();
    this.seleccionarRol();
  },
  methods: {
    uppercase() {
      this.usuario = this.usuario.toUpperCase();
    },
    listar() {
      let me = this;
      let header = { Authorization: "Bearer " + this.$store.state.token };
      let configuracion = { headers: header };
      axios
        .get("api/Usuarios/Listar", configuracion)
        .then(function (response) {
          me.usuarios = response.data;
        })
        .catch(function (error) {
          if (error.response.status == 401) {
            me.$store.dispatch("salir");
          } else {
            console.log(error);
          }
        });
    },
    seleccionarRol() {
      let me = this;
      let header = { Authorization: "Bearer " + this.$store.state.token };
      let configuracion = { headers: header };
      var rolesArray = [];
      axios
        .get("api/Roles/Seleccionar", configuracion)
        .then(function (response) {
          rolesArray = response.data;
          rolesArray.map(function (x) {
            me.roles.push({ text: x.rol, value: x.idRol });
          });
        })
        .catch(function (error) {
          if (error.response.status == 401) {
            me.$store.dispatch("salir");
          } else {
            console.log(error);
          }
        });
    },
    seleccionarPersonal() {
      let me = this;
      let header = { Authorization: "Bearer " + this.$store.state.token };
      let configuracion = { headers: header };
      var personalesArray = [];
      axios
        .get("api/Personales/Seleccionar", configuracion)
        .then(function (response) {
          personalesArray = response.data;
          personalesArray.map(function (x) {
            me.personales.push({ text: x.personal, value: x.idPersonal });
          });
        })
        .catch(function (error) {
          if (error.response.status == 401) {
            me.$store.dispatch("salir");
          } else {
            console.log(error);
          }
        });
    },
    editar(item) {
      this.id = item.idUsuario;
      this.idrol = item.idRol;
      this.idpersonal = item.idPersonal;
      this.usuario = item.usuario;
      this.password = item.passwordHash;
      this.anteriorPassword = item.passwordHash;
      this.editedIndex = 1;
      this.dialog = true;
    },
    cerrar() {
      this.dialog = false;
      this.limpiar();
      this.$refs.form.resetValidation();
    },
    limpiar() {
      this.id = "";
      this.idrol = "";
      this.idpersonal = "";
      this.usuario = "";
      this.password = "";
      this.anteriorPassword = "";
      this.actualizarPassword = false;
      this.editedIndex = -1;
      this.error = null;
      this.personales = [];
    },
    guardar() {
      if (this.$refs.form.validate()) {
        let header = { Authorization: "Bearer " + this.$store.state.token };
        let configuracion = { headers: header };
        if (this.editedIndex > -1) {
          let me = this;
          if (me.password != me.anteriorPassword) {
            me.actualizarPassword = true;
          }
          axios
            .put(
              "api/Usuarios/Actualizar",
              {
                idUsuario: me.id,
                idRol: me.idrol,
                idPersonal: me.idpersonal,
                usuario: me.usuario,
                password: me.password,
                actualizarPassword: me.actualizarPassword,
              },
              configuracion
            )
            .then(function (response) {
              me.cerrar();
              me.listar();
              me.limpiar();
            })
            .catch(function (error) {
              console.log(error);
            });
        } else {
          let me = this;
          axios
            .post(
              "api/Usuarios/Crear",
              {
                idRol: me.idrol,
                idPersonal: me.idpersonal,
                usuario: me.usuario,
                password: me.password,
                estado: "Activo",
              },
              configuracion
            )
            .then(function (response) {
              me.cerrar();
              me.listar();
              me.limpiar();
            })
            .catch(function (error) {
              console.log(error);
            });
        }
      }
    },
    activardesactivarMostrar(accion, item) {
      (this.adModal = 1), (this.adUsuario = item.usuario);
      this.adId = item.idUsuario;
      if (accion == 1) {
        this.adAccion = 1;
      } else if (accion == 2) {
        this.adAccion = 2;
      } else {
        this.adModal = 0;
      }
    },
    activardesactivarCerrar() {
      this.adModal = 0;
    },
    activar() {
      let me = this;
      let header = { Authorization: "Bearer " + this.$store.state.token };
      let configuracion = { headers: header };
      axios
        .put("api/Usuarios/Activar/" + this.adId, {}, configuracion)
        .then(function (response) {
          me.adModal = 0;
          me.adAccion = 0;
          me.adUsuario = "";
          me.adId = "";
          me.listar();
        })
        .catch(function (error) {
          if (error.response.status == 401) {
            me.$store.dispatch("salir");
          } else {
            console.log(error);
          }
        });
    },
    desactivar() {
      let me = this;
      let header = { Authorization: "Bearer " + this.$store.state.token };
      let configuracion = { headers: header };
      axios
        .put("api/Usuarios/Desactivar/" + this.adId, {}, configuracion)
        .then(function (response) {
          me.adModal = 0;
          me.adAccion = 0;
          me.adUsuario = "";
          me.adId = "";
          me.listar();
        })
        .catch(function (error) {
          if (error.response.status == 401) {
            me.$store.dispatch("salir");
          } else {
            console.log(error);
          }
        });
    },
  },
};
</script>
