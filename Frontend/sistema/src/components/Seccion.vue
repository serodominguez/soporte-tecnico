<template>
  <v-layout align-start>
    <v-flex>
      <v-toolbar flat color="white">
        <v-toolbar-title>Secciónes</v-toolbar-title>
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
          <v-btn slot="activator" color="primary" dark class="mb-2"
            >Nuevo</v-btn
          >
          <v-card>
            <v-card-title>
              <span class="headline">{{ formTitle }}</span>
            </v-card-title>
            <v-form ref="form">
              <v-container grid-list-md>
                <v-layout wrap>
                  <v-flex xs12 sm12 md12 lg12 xl12>
                    <v-text-field
                      v-model="seccion"
                      :rules="[rules.requerido, rules.contador]"
                      @keyup="uppercase"
                      maxlength="30"
                      label="Sección"
                    ></v-text-field>
                  </v-flex>
                  <v-flex xs12 sm12 md12 lg12 xl12>
                    <v-select v-model="idarea" :items="areas" label="Area">
                    </v-select>
                  </v-flex>
                  <v-flex xs12 sm12 md12 lg12 xl12>
                    <v-text-field
                      v-model.number="pkseccion"
                      :rules="[rules.requerido]"
                      @keyup="uppercase"
                      onkeypress="return (event.charCode >= 48 && event.charCode <= 57)"
                      label="Código"
                    ></v-text-field>
                  </v-flex>
                  <v-flex class="red--text font-weight-bold" v-if="error"
                    ><strong>{{ error }}</strong></v-flex
                  >
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
              el ítem {{ adSeccion }}
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
        :items="secciones"
        :search="search"
        :rows-per-page-text="paginas"
        class="elevation-1"
      >
        <template slot="items" slot-scope="props">
          <td>{{ props.item.seccion }}</td>
          <td>{{ props.item.pkSeccion }}</td>
          <td>{{ props.item.area }}</td>
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
      secciones: [],
      areas: [],
      dialog: false,
      headers: [
        { text: "Sección", value: "seccion" },
        { text: "Código", value: "pkSeccion", sortable: false },
        { text: "Area", value: "area", sortable: false },
        { text: "Estado", value: "estado", sortable: false },
        { text: "Opciones", value: "opciones", sortable: false },
      ],
      paginas: "Secciones por Página",
      search: "",
      editedIndex: -1,
      id: "",
      idarea: 0,
      seccion: "",
      pkseccion: "",
      valida: 0,
      validaMensaje: [],
      adModal: 0,
      adAccion: 0,
      adSeccion: "",
      adId: "",
      error: null,
      rules: {
        requerido: (value) => !!value || "Requerido!",
        contador: (value) => value.length <= 30 || "Máximo 30 caracteres",
      },
    };
  },
  computed: {
    formTitle() {
      return this.editedIndex === -1 ? "Nueva sección" : "Actualizar sección";
    },
  },

  watch: {
    dialog(val) {
      val || this.cerrar();
    },
  },
  created() {
    this.listar();
    this.seleccionar();
  },

  methods: {
    uppercase() {
      this.seccion = this.seccion.toUpperCase();
    },
    listar() {
      let me = this;
      let header = { Authorization: "Bearer " + this.$store.state.token };
      let configuracion = { headers: header };
      axios
        .get("api/Secciones/ListarInternos", configuracion)
        .then(function (response) {
          me.secciones = response.data;
        })
        .catch(function (error) {
          if (error.response.status == 401) {
            me.$store.dispatch("salir");
          } else {
            console.log(error);
          }
        });
    },
    seleccionar() {
      let me = this;
      let header = { Authorization: "Bearer " + this.$store.state.token };
      let configuracion = { headers: header };
      var areasArray = [];
      axios
        .get("api/Areas/SeleccionarInternos", configuracion)
        .then(function (response) {
          areasArray = response.data;
          areasArray.map(function (x) {
            me.areas.push({ text: x.area, value: x.idArea });
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
      this.id = item.idSeccion;
      this.idarea = item.idArea;
      this.seccion = item.seccion;
      this.pkseccion = item.pkSeccion;
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
      this.idarea = 0;
      this.seccion = "";
      this.pkseccion = "";
      this.editedIndex = -1;
      this.error = null;
    },
    guardar() {
      if (this.$refs.form.validate()) {
        let header = { Authorization: "Bearer " + this.$store.state.token };
        let configuracion = { headers: header };
        if (this.editedIndex > -1) {
          this.error = null;
          let me = this;
          axios
            .put(
              "api/Secciones/ActualizarInternos",
              {
                idSeccion: me.id,
                idarea: me.idarea,
                seccion: me.seccion,
                pkseccion: me.pkseccion,
              },
              configuracion
            )
            .then(function (response) {
              me.cerrar();
              me.listar();
              me.limpiar();
            })
            .catch((err) => {
              if (err.response.status == 400) {
                this.error = "El código de sección ya se encuentra registrado!";
              }
            });
        } else {
          let me = this;
          axios
            .post(
              "api/Secciones/CrearInternos",
              {
                idarea: me.idarea,
                seccion: me.seccion,
                direccion: "Av. Thomas Bata Quillacollo",
                pkseccion: me.pkseccion,
                estado: "Activo",
              },
              configuracion
            )
            .then(function (response) {
              me.cerrar();
              me.listar();
              me.limpiar();
            })
            .catch((err) => {
              if (err.response.status == 400) {
                this.error = "El código de sección ya se encuentra registrado!";
              }
            });
        }
      }
    },
    activardesactivarMostrar(accion, item) {
      (this.adModal = 1), (this.adSeccion = item.seccion);
      this.adId = item.idSeccion;
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
        .put("api/Secciones/Activar/" + this.adId, {}, configuracion)
        .then(function (response) {
          me.adModal = 0;
          me.adAccion = 0;
          me.adSeccion = "";
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
        .put("api/Secciones/Desactivar/" + this.adId, {}, configuracion)
        .then(function (response) {
          me.adModal = 0;
          me.adAccion = 0;
          me.adSeccion = "";
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
