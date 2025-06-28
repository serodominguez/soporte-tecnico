<template>
  <v-layout align-start>
    <v-flex>
      <v-toolbar flat color="white">
        <v-toolbar-title>Licencias</v-toolbar-title>
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
                  <v-flex xs6 sm6 md6 lg6 xl6>
                    <v-select
                      v-model="idproveedor"
                      :rules="[rules.requerido]"
                      :items="proveedores"
                      label="Proveedor"
                      no-data-text="No hay datos disponibles"
                    ></v-select>
                  </v-flex>
                  <v-flex xs12 sm12 md6 lg6 xl6>
                    <v-text-field
                      v-model="programa"
                      :rules="[rules.requerido]"
                      @keyup="uppercase"
                      maxlength="50"
                      label="Programa"
                    ></v-text-field>
                  </v-flex>
                  <v-flex xs12 sm12 md6 lg6 xl6>
                    <v-text-field
                      v-model="licencia"
                      @keyup="uppercase"
                      maxlength="100"
                      label="Licencia"
                    ></v-text-field>
                  </v-flex>
                  <v-flex xs12 sm12 md6 lg6 xl6>
                    <v-select
                      v-model="tipolicencia"
                      :items="tipos"
                      label="Tipo de licencia"
                    ></v-select>
                  </v-flex>
                  <v-flex xs12 sm12 md6 lg6 xl6>
                    <v-text-field
                      v-model.number="cantidadequipos"
                      :rules="[rules.requerido]"
                      onkeypress="return (event.charCode >= 48 && event.charCode <= 57)"
                      label="Cantidad de Licencias"
                    ></v-text-field>
                  </v-flex>
                  <v-flex xs12 sm12 md6 lg6 xl6>
                    <v-select
                      v-model="moneda"
                      :items="monedas"
                      label="Moneda"
                    ></v-select>
                  </v-flex>
                  <v-flex xs12 sm12 md6 lg6 xl6>
                    <v-text-field
                      v-model.number="preciocompra"
                      onkeypress="return (event.charCode >= 48 && event.charCode <= 57)"
                      label="Precio de compra"
                    ></v-text-field>
                  </v-flex>
                  <v-flex xs12 sm12 md6 lg6 xl6>
                    <v-text-field
                      mask="##/##/####"
                      placeholder="Fecha de compra"
                      v-model="fechacompra"
                      @keyup="validarFechaCompra"
                      return-masked-value
                    ></v-text-field>
                    <div
                      class="red--text font-weight-bold"
                      v-text="validaCompra"
                    ></div>
                  </v-flex>
                  <v-flex xs12 sm12 md6 lg6 xl6>
                    <v-text-field
                      mask="##/##/####"
                      placeholder="Fecha de activación"
                      v-model="fechaactivacion"
                      @keyup="validarFechaActivacion"
                      return-masked-value
                    ></v-text-field>
                    <div
                      class="red--text font-weight-bold"
                      v-text="validaActivacion"
                    ></div>
                  </v-flex>
                  <v-flex xs12 sm12 md6 lg6 xl6>
                    <v-text-field
                      mask="##/##/####"
                      placeholder="Fecha de caducidad"
                      v-model="fechacaducidad"
                      @keyup="validarFechaCaducidad"
                      return-masked-value
                    ></v-text-field>
                    <div
                      class="red--text font-weight-bold"
                      v-text="validaCaducidad"
                    ></div>
                  </v-flex>
                  <v-flex xs12 sm12 md12 lg12 xl12>
                    <v-text-field
                      v-model="comentarios"
                      @keyup="uppercase"
                      maxlength="140"
                      label="Comentarios"
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
              el ítem {{ adLicencia }}
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
        :items="licencias"
        :search="search"
        :rows-per-page-text="paginas"
        class="elevation-1"
      >
        <template slot="items" slot-scope="props">
          <td>{{ props.item.proveedor }}</td>
          <td>{{ props.item.programa }}</td>
          <td>{{ props.item.licencia }}</td>
          <td>{{ props.item.tipoLicencia }}</td>
          <td>{{ props.item.cantidadEquipos }}</td>
          <td>{{ props.item.precioCompra }}</td>
          <td>{{ props.item.moneda }}</td>
          <td>{{ props.item.fechaCompra }}</td>
          <td>{{ props.item.fechaCaducidad }}</td>
          <td>{{ props.item.fechaActivacion }}</td>
          <td>{{ props.item.comentarios }}</td>
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
import * as moment from "moment";

export default {
  data() {
    return {
      licencias: [],
      proveedores: [],
      dialog: false,
      headers: [
        { text: "Proveedor", value: "proveedor" },
        { text: "Programa", value: "programa" },
        { text: "Licencia", value: "licencia", sortable: false },
        { text: "Tipo de Licencia", value: "tipolicencia", sortable: false },
        { text: "Cantidad de Equipos", value: "cantidadequipos", sortable: false },
        { text: "Precio de Compra", value: "preciocompra", sortable: false },
        { text: "Moneda", value: "moneda" },
        { text: "Fecha de Compra", value: "fechaCompra", sortable: false },
        { text: "Fecha de Caducidad", value: "fechaCaducidad", sortable: false },
        { text: "Fecha de Activación", value: "fechaActivacion", sortable: false },
        { text: "Comentarios", value: "comentarios", sortable: false },
        { text: "Estado", value: "estado", sortable: false },
        { text: "Opciones", value: "opciones", sortable: false },
      ],
      paginas: "Licencias por Página",
      monedas: ["BOLIVIANOS", "DOLARES"],
      tipos: ["MENSUAL", "ANUAL", "PERPETUA"],
      search: "",
      editedIndex: -1,
      id: "",
      idproveedor: "",
      programa: "",
      licencia: "",
      comentarios: "",
      tipolicencia: "",
      cantidadequipos: "",
      preciocompra: "",
      moneda: "",
      fechacompra: "",
      fechacaducidad: "",
      fechaactivacion: "",
      validaCompra: "",
      validaCaducidad: "",
      validaActivacion: "",
      validaFecha: "",
      adModal: 0,
      adAccion: 0,
      adLicencia: "",
      adId: "",
      error: null,
      rules: {
        requerido: (value) => !!value || "Requerido!",
      },
    };
  },
  computed: {
    formTitle() {
      return this.editedIndex === -1 ? "Nueva licencia" : "Actualizar licencia";
    },
  },

  watch: {
    dialog(val) {
      val || this.cerrar();
    },
  },
  created() {
    this.listar();
    this.seleccionarProveedor();
  },

  methods: {
    uppercase() {
      this.programa = this.programa.toUpperCase();
      this.tipolicencia = this.tipolicencia.toUpperCase();
      this.licencia = this.licencia.toUpperCase();
      this.comentarios = this.comentarios.toUpperCase();
    },
    validarFecha() {
      this.validaActivacion = "";
      var activacion = moment(
        this.fechaactivacion,
        "DD/MM/YYYY",
        true
      ).isValid();
      var caducidad = moment(this.fechacaducidad, "DD/MM/YYYY", true).isValid();
      if (activacion <= caducidad) {
        this.validaFecha =
          "La fecha de caducidad no puede ser menor a la de activación";
      }
    },
    validarFechaCompra() {
      this.validaCompra = "";
      var result = moment(this.fechacompra, "DD/MM/YYYY", true).isValid();
      if (result != true) {
        this.validaCompra = "Fecha incorrecta!";
      }
    },
    validarFechaCaducidad() {
      this.validaCaducidad = "";
      var result = moment(this.fechacaducidad, "DD/MM/YYYY", true).isValid();
      if (result != true) {
        this.validaCaducidad = "Fecha incorrecta!";
      }
    },
    validarFechaActivacion() {
      this.validaActivacion = "";
      var result = moment(this.fechaactivacion, "DD/MM/YYYY", true).isValid();
      if (result != true) {
        this.validaActivacion = "Fecha incorrecta!";
      }
    },
    listar() {
      let me = this;
      let header = { Authorization: "Bearer " + this.$store.state.token };
      let configuracion = { headers: header };
      axios
        .get("api/Licencias/Listar", configuracion)
        .then(function (response) {
          me.licencias = response.data;
        })
        .catch(function (error) {
          if (error.response.status == 401) {
            me.$store.dispatch("salir");
          } else {
            console.log(error);
          }
        });
    },
    seleccionarProveedor() {
      let me = this;
      let header = { Authorization: "Bearer " + this.$store.state.token };
      let configuracion = { headers: header };
      var proveedoresArray = [];
      axios
        .get("api/Proveedores/Seleccionar", configuracion)
        .then(function (response) {
          proveedoresArray = response.data;
          proveedoresArray.map(function (x) {
            me.proveedores.push({ text: x.razonSocial, value: x.idProveedor });
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
      this.id = item.idLicencia;
      this.programa = item.programa;
      this.licencia = item.licencia;
      this.tipolicencia = item.tipoLicencia;
      this.cantidadequipos = item.cantidadEquipos;
      this.preciocompra = item.precioCompra;
      this.fechacompra = item.fechaCompra;
      this.fechacaducidad = item.fechaCaducidad;
      this.fechaactivacion = item.fechaActivacion;
      this.idproveedor = item.idProveedor;
      this.moneda = item.moneda;
      this.comentarios = item.comentarios;
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
      this.programa = "";
      this.licencia = "";
      this.tipolicencia = "";
      this.cantidadequipos = "";
      this.preciocompra = "";
      this.fechacompra = "";
      this.fechacaducidad = "";
      this.fechaactivacion = "";
      this.idproveedor = "";
      this.moneda = "";
      this.comentarios = "";
      this.editedIndex = -1;
      this.menu = false;
      this.error = null;
      this.validaCompra = "";
      this.validaCaducidad = "";
      this.validaActivacion = "";
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
              "api/Licencias/Actualizar",
              {
                idlicencia: me.id,
                programa: me.programa,
                licencia: me.licencia,
                tipolicencia: me.tipolicencia,
                cantidadequipos: me.cantidadequipos,
                preciocompra: me.preciocompra,
                fechacompra: me.fechacompra,
                fechacaducidad: me.fechacaducidad,
                fechaactivacion: me.fechaactivacion,
                idproveedor: me.idproveedor,
                moneda: me.moneda,
                comentarios: me.comentarios
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
                this.error = "El número de serie ya se encuentra registrado!";
              }
            });
        } else {
          let me = this;
          axios
            .post(
              "api/Licencias/Crear",
              {
                programa: me.programa,
                licencia: me.licencia,
                tipolicencia: me.tipolicencia,
                cantidadequipos: me.cantidadequipos,
                preciocompra: me.preciocompra,
                fechacompra: me.fechacompra,
                fechacaducidad: me.fechacaducidad,
                fechaactivacion: me.fechaactivacion,
                estado: "Activo",
                idproveedor: me.idproveedor,
                moneda: me.moneda,
                comentarios: me.comentarios
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
                this.error = "El número de serie ya se encuentra registrado!";
              }
            });
        }
      }
    },
    activardesactivarMostrar(accion, item) {
      (this.adModal = 1), (this.adLicencia = item.programa);
      this.adId = item.idLicencia;
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
        .put("api/Licencias/Activar/" + this.adId, {}, configuracion)
        .then(function (response) {
          me.adModal = 0;
          me.adAccion = 0;
          me.adLicencia = "";
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
        .put("api/Licencias/Desactivar/" + this.adId, {}, configuracion)
        .then(function (response) {
          me.adModal = 0;
          me.adAccion = 0;
          me.adLicencia = "";
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
