<template>
  <v-layout align-start>
    <v-flex>
      <v-toolbar flat color="white">
        <v-toolbar-title>Consignatarios</v-toolbar-title>
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
                  <v-flex xs12 sm12 md6 lg6 xl6>
                    <v-text-field
                      v-model="nombres"
                      :rules="[rules.requerido, rules.contador]"
                      @keyup="uppercase"
                      maxlength="30"
                      label="Nombres"
                    ></v-text-field>
                  </v-flex>
                  <v-flex xs12 sm12 md6 lg6 xl6>
                    <v-text-field
                      v-model="apellidos"
                      :rules="[rules.requerido, rules.contador]"
                      @keyup="uppercase"
                      maxlength="30"
                      label="Apellidos"
                    ></v-text-field>
                  </v-flex>
                  <v-flex xs12 sm12 md6 lg6 xl6>
                    <v-text-field
                      v-model="pkempleado"
                      @keyup="uppercase"
                      label="Código"
                    ></v-text-field>
                  </v-flex>
                  <v-flex xs12 sm12 md6 lg6 xl6>
                    <v-text-field
                      v-model="cargo"
                      :rules="[rules.requerido]"
                      @keyup="uppercase"
                      maxlength="50"
                      label="Cargo"
                    ></v-text-field>
                  </v-flex>
                  <v-flex xs12 sm12 md6 lg6 xl6>
                    <v-text-field
                      v-model="carnet"
                      :rules="[rules.requerido]"
                      onkeypress="return (event.charCode >= 48 && event.charCode <= 57)"
                      maxlength="10"
                      label="Carnet"
                    ></v-text-field>
                  </v-flex>
                  <v-flex xs12 sm12 md6 lg6 xl6>
                    <v-text-field
                      v-model="celular"
                      onkeypress="return (event.charCode >= 48 && event.charCode <= 57)"
                      maxlength="10"
                      label="Celular"
                    ></v-text-field>
                  </v-flex>
                  <v-flex xs12 sm12 md6 lg6 xl6>
                    <v-text-field
                      v-model="telefono"
                      onkeypress="return (event.charCode >= 48 && event.charCode <= 57)"
                      maxlength="10"
                      label="Teléfono"
                    ></v-text-field>
                  </v-flex>
                  <v-flex xs12 sm12 md6 lg6 xl6>
                    <v-autocomplete
                      v-model="idseccion"
                      :items="secciones"
                      :rules="[rules.requerido]"
                      label="Tienda"
                      no-data-text="No hay datos disponibles"
                    >
                    </v-autocomplete>
                  </v-flex>
                  <v-flex xs12 sm12 md12 lg12 xl12>
                    <v-text-field
                      v-model="direccion"
                       @keyup="uppercase"
                      :rules="[rules.requerido]"
                      label="Dirección"
                    >
                    </v-text-field>
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
              el ítem {{ adPersonal }}
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
        :items="personales"
        :search="search"
        :rows-per-page-text="paginas"
        class="elevation-1"
      >
        <template slot="items" slot-scope="props">
          <td>{{ props.item.nombres }}</td>
          <td>{{ props.item.apellidos }}</td>
          <td>{{ props.item.cargo }}</td>
          <td>{{ props.item.seccion }}</td>
          <td>{{ props.item.tipo }}</td>
          <td>{{ props.item.celular }}</td>
          <td>{{ props.item.telefono }}</td>
          <td>{{ props.item.pkEmpleado }}</td>
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
      personales: [],
      secciones: [],
      dialog: false,
      headers: [
        { text: "Nombres", value: "nombres" },
        { text: "Apellidos", value: "apellidos", sortable: false },
        { text: "Cargo", value: "cargo", sortable: false },
        { text: "Sección", value: "seccion", sortable: false },
        { text: "Tipo", value: "tipo", sortable: false },
        { text: "Celular", value: "celular", sortable: false },
        { text: "Teléfono", value: "telefono", sortable: false },
        { text: "Código", value: "pkEmpleado", sortable: false },
        { text: "Estado", value: "estado", sortable: false },
        { text: "Opciones", value: "opciones", sortable: false },
      ],
      paginas: "Consignatarios por Página",
      search: "",
      editedIndex: -1,
      id: "",
      idseccion: 0,
      nombres: "",
      apellidos: "",
      cargo: "",
      celular: "",
      carnet: "",
      direccion: "",
      telefono: "",
      pkempleado: "",
      valida: 0,
      validaMensaje: [],
      adModal: 0,
      adAccion: 0,
      adPersonal: "",
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
      return this.editedIndex === -1
        ? "Nuevo consignatario"
        : "Actualizar consignatario";
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
      this.nombres = this.nombres.toUpperCase();
      this.cargo = this.cargo.toUpperCase();
      this.apellidos = this.apellidos.toUpperCase();
      this.direccion = this.direccion.toUpperCase();
      this.pkempleado = this.pkempleado.toUpperCase();
    },
    listar() {
      let me = this;
      let header = { Authorization: "Bearer " + this.$store.state.token };
      let configuracion = { headers: header };
      axios
        .get("api/Personales/ListarConsignatario", configuracion)
        .then(function (response) {
          me.personales = response.data;
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
      var seccionesArray = [];
      axios
        .get("api/Secciones/SeleccionarTiendas", configuracion)
        .then(function (response) {
          seccionesArray = response.data;
          seccionesArray.map(function (x) {
            me.secciones.push({ text: x.seccion, value: x.idSeccion });
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
      this.id = item.idPersonal;
      this.idseccion = item.idSeccion;
      this.nombres = item.nombres;
      this.apellidos = item.apellidos;
      this.cargo = item.cargo;
      this.celular = item.celular;
      this.carnet = item.carnet;
      this.direccion = item.direccion;
      this.telefono = item.telefono;
      this.pkempleado = item.pkEmpleado;
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
      this.idseccion = 0;
      this.nombres = "";
      this.apellidos = "";
      this.cargo = "";
      this.celular = "";
      this.celular = "";
      this.carnet = "";
      this.telefono = "";
      this.pkempleado = "";
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
              "api/Personales/Actualizar",
              {
                idpersonal: me.id,
                idseccion: me.idseccion,
                nombres: me.nombres,
                apellidos: me.apellidos,
                cargo: me.cargo,
                tipo: "CONSIGNATARIO",
                celular: me.celular,
                carnet: me.carnet,
                direccion: me.direccion,
                telefono: me.telefono,
                pkempleado: me.pkempleado,
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
                this.error =
                  "El código del consignatario ya se encuentra registrado!";
              }
            });
        } else {
          let me = this;
          axios
            .post(
              "api/Personales/Crear",
              {
                idseccion: me.idseccion,
                nombres: me.nombres,
                apellidos: me.apellidos,
                cargo: me.cargo,
                tipo: "CONSIGNATARIO",
                celular: me.celular,
                carnet: me.carnet,
                direccion: me.direccion,
                telefono: me.telefono,
                pkempleado: me.pkempleado,
                cuenta: "No",
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
                this.error =
                  "El código del consignatario ya se encuentra registrado!";
              }
            });
        }
      }
    },
    activardesactivarMostrar(accion, item) {
      (this.adModal = 1), (this.adPersonal = item.nombres);
      this.adId = item.idPersonal;
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
        .put("api/Personales/Activar/" + this.adId, {}, configuracion)
        .then(function (response) {
          me.adModal = 0;
          me.adAccion = 0;
          me.adPersonal = "";
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
        .put("api/Personales/Desactivar/" + this.adId, {}, configuracion)
        .then(function (response) {
          me.adModal = 0;
          me.adAccion = 0;
          me.adPersonal = "";
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
