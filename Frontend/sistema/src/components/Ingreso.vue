<template>
  <v-layout align-start>
    <v-flex>
      <v-toolbar flat color="white">
        <v-toolbar-title>Ingresos {{ this.numeroingreso }} </v-toolbar-title>
        <v-divider class="mx-2" inset vertical></v-divider>
        <v-spacer></v-spacer>
        <v-text-field
          v-if="verNuevo == 0"
          class="text-xs-center"
          v-model="search"
          append-icon="search"
          label="Búsqueda"
          single-line
          hide-details
        ></v-text-field>
        <v-spacer></v-spacer>
        <v-btn
          v-if="verNuevo == 0"
          @click="mostrarNuevo"
          color="primary"
          dark
          class="mb-2"
          >Nuevo</v-btn
        >
        <v-dialog v-model="verEquipos" persistent max-width="1000px">
          <v-card>
            <v-card-title>
              <span class="headline">Seleccione un Equipo</span>
            </v-card-title>
            <v-card-text>
              <v-container grid-list-md>
                <v-layout wrap>
                  <v-flex xs12 sm12 md12 lg12 xl12>
                    <v-text-field
                      append-icon="search"
                      class="text-xs-center"
                      v-model="texto"
                      label="Ingrese el equipo a buscar"
                      @keyup.enter="buscarEquipo()"
                    ></v-text-field>
                    <template>
                      <v-data-table
                        :headers="cabeceraEquipos"
                        :items="equipos"
                        :rows-per-page-text="pagina"
                        class="elevation-1"
                      >
                        <template slot="items" slot-scope="props">
                          <td>{{ props.item.categoria }}</td>
                          <td>{{ props.item.marca }}</td>
                          <td>{{ props.item.modelo }}</td>
                          <td>{{ props.item.serie }}</td>
                          <td>{{ props.item.codigoActivo }}</td>
                          <td>
                            <v-icon
                              small
                              class="mr-2"
                              @click="agregarDetalle(props.item)"
                              >add</v-icon
                            >
                          </td>
                        </template>
                        <template slot="no-data">
                          <h3>No hay equipos para mostrar.</h3>
                        </template>
                      </v-data-table>
                    </template>
                  </v-flex>
                </v-layout>
              </v-container>
            </v-card-text>
            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn @click="ocultarEquipos()" color="error" flat
                >Cerrar</v-btn
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
              >Anular Item?</v-card-title
            >
            <v-card-text>
              Estás a punto de
              <span v-if="adAccion == 1">Activar</span>
              <span v-if="adAccion == 2">Anular</span>
              el ítem {{ adIngreso }}
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
                >Anular</v-btn
              >
            </v-card-actions>
          </v-card>
        </v-dialog>
      </v-toolbar>
      <v-data-table
        :headers="headers"
        :items="ingresos"
        :search="search"
        :rows-per-page-text="paginas"
        class="elevation-1"
        v-if="verNuevo == 0"
      >
        <template slot="items" slot-scope="props">
          <td>{{ props.item.usuario }}</td>
          <td>{{ props.item.proveedor }}</td>
          <td>{{ props.item.numeroIngreso }}</td>
          <td>{{ props.item.fechaIngreso }}</td>
          <td>{{ props.item.tipoComprobante }}</td>
          <td>{{ props.item.numeroComprobante }}</td>
          <td>{{ props.item.numeroOrden }}</td>
          <td>{{ props.item.autorizado }}</td>
          <td>{{ props.item.observaciones }}</td>
          <td>
            <div v-if="props.item.estado == 'Ingresado'">
              <span class="green--text">{{ props.item.estado }}</span>
            </div>
            <div v-else>
              <span class="red--text">{{ props.item.estado }}</span>
            </div>
          </td>
          <td>
            <v-icon small class="mr-2" @click="verDetalles(props.item)"
              >tab</v-icon
            >
            <!-- <template v-if="props.item.estado == 'Ingresado'">
              <v-icon small @click="activardesactivarMostrar(2, props.item)"
                >block</v-icon
              >
            </template> -->
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
      <v-form ref="form">
        <v-container grid-list-sm class="pa-4 white" v-if="verNuevo">
          <v-layout row wrap>
            <v-flex xs6 sm6 md4 lg4 xl4>
              <v-select
                v-if="this.verDet == 1"
                v-model="tipocomprobante"
                :rules="[rules.requerido]"
                :items="comprobantes"
                label="Tipo de comprobante"
                readonly
              ></v-select>
              <v-select
                v-else
                v-model="tipocomprobante"
                :rules="[rules.requerido]"
                :items="comprobantes"
                label="Tipo de comprobante"
              ></v-select>
            </v-flex>
            <v-flex xs6 sm6 md4 lg4 xl4>
              <v-text-field
                v-if="this.verDet == 1"
                v-model="numerocomprobante"
                :rules="[rules.requerido]"
                onkeypress="return (event.charCode >= 48 && event.charCode <= 57)"
                maxlength="10"
                label="Número de comprobante"
                readonly
              ></v-text-field>
              <v-text-field
                v-else
                v-model="numerocomprobante"
                :rules="[rules.requerido]"
                onkeypress="return (event.charCode >= 48 && event.charCode <= 57)"
                maxlength="10"
                label="Número de comprobante"
              ></v-text-field>
            </v-flex>
            <v-flex xs6 sm6 md4 lg4 xl4>
              <v-autocomplete
                v-if="this.verDet == 1"
                v-model="idproveedor"
                :rules="[rules.requerido]"
                :items="proveedores"
                label="Proveedor"
                readonly
              ></v-autocomplete>
              <v-autocomplete
                v-else
                v-model="idproveedor"
                :rules="[rules.requerido]"
                :items="proveedores"
                label="Proveedor"
              ></v-autocomplete>
            </v-flex>
            <v-flex xs6 sm6 md4 lg4 xl4>
              <v-text-field
                v-if="this.verDet == 1"
                v-model="numeroorden"
                :rules="[rules.requerido]"
                @keyup="uppercase"
                maxlength="10"
                label="Orden de compra"
                readonly
              ></v-text-field>
              <v-text-field
                v-else
                v-model="numeroorden"
                :rules="[rules.requerido]"
                @keyup="uppercase"
                maxlength="10"
                label="Orden de compra"
              ></v-text-field>
            </v-flex>
            <v-flex xs6 sm6 md4 lg4 xl4>
              <v-text-field
                v-if="this.verDet == 1"
                v-model="autorizado"
                :rules="[rules.requerido]"
                @keyup="uppercase"
                maxlength="60"
                label="Pedido por"
                readonly
              ></v-text-field>
              <v-text-field
                v-else
                v-model="autorizado"
                :rules="[rules.requerido]"
                @keyup="uppercase"
                maxlength="60"
                label="Pedido por"
              ></v-text-field>
            </v-flex>
            <v-flex xs6 sm6 md4 lg4 xl4>
              <v-text-field
                v-if="this.verDet == 1"
                v-model="observaciones"
                @keyup="uppercase"
                maxlength="100"
                label="Observaciones"
                readonly
              ></v-text-field>
              <v-text-field
                v-else
                v-model="observaciones"
                @keyup="uppercase"
                maxlength="100"
                label="Observaciones"
              ></v-text-field>
            </v-flex>
            <v-flex xs12 sm12 md8 lg8 xl8>
              <v-text-field
                @keyup="uppercase"
                @keyup.enter="buscarSerie()"
                v-model="serie"
                label="Número de serie"
                v-if="verDet == 0"
              ></v-text-field>
            </v-flex>
            <v-flex xs12 sm2 md2 lg2 xl2>
              <v-btn
                @click="mostrarEquipos()"
                fab
                dark
                color="teal"
                v-if="verDet == 0"
              >
                <v-icon dark>list</v-icon>
              </v-btn>
            </v-flex>
            <v-flex xs12 sm2 md2 lg2 xl2 v-if="errorEquipo">
              <div class="red--text" v-text="errorEquipo"></div>
            </v-flex>
            <v-flex xs12 sm12 md12 lg12 xl12>
              <v-data-table
                :headers="cabeceraDetalles"
                :items="detalles"
                hide-actions
                class="elevation-1"
              >
                <template slot="items" slot-scope="props">
                  <td>{{ props.item.idEquipo }}</td>
                  <td>{{ props.item.categoria }}</td>
                  <td>{{ props.item.marca }}</td>
                  <td>{{ props.item.modelo }}</td>
                  <td>{{ props.item.serie }}</td>
                  <td>{{ props.item.codigoActivo }}</td>
                  <td v-if="verDet == 0">
                    <v-icon
                      small
                      class="mr-2"
                      @click="eliminarDetalle(detalles, props.item)"
                      >delete</v-icon
                    >
                  </td>
                </template>
                <template slot="no-data">
                  <h3>No hay equipos agregados al detalle.</h3>
                </template>
              </v-data-table>
            </v-flex>
            <v-flex>
              <v-flex xs12 sm12 md12 lg12 xl12>
                <div
                  class="red--text"
                  v-for="v in validaMensaje"
                  :key="v"
                  v-text="v"
                ></div>
              </v-flex>
              <v-flex xs12 sm12 md12 lg12 xl12>
                <v-btn
                  :disabled="load"
                  :loading="load"
                  v-if="verDet == 0"
                  @click="guardar()"
                  color="success"
                  >Guardar</v-btn
                >
                <v-btn @click="ocultarNuevo()" color="error">Cancelar</v-btn>
              </v-flex>
            </v-flex>
          </v-layout>
        </v-container>
      </v-form>
    </v-flex>
  </v-layout>
</template>
<script>
import axios from "axios";
export default {
  data() {
    return {
      load: false,
      ingresos: [],
      proveedores: [],
      detalles: [],
      equipos: [],
      dialog: false,
      headers: [
        { text: "Usuario", value: "usuario" },
        { text: "Proveedor", value: "proveedor" },
        { text: "Número de Ingreso", value: "numeroIngreso", sortable: false },
        { text: "Fecha de Ingreso", value: "fechaIngreso", sortable: false },
        {
          text: "Tipo de Comprobante",
          value: "tipoComprobante",
          sortable: false,
        },
        {
          text: "Número de Comprobante",
          value: "numeroComprobante",
          sortable: false,
        },
        { text: "Orden de Compra", value: "numeroOrden", sortable: false },
        { text: "Pedido por", value: "autorizado", sortable: false },
        { text: "Observaciones", value: "observaciones", sortable: false },
        { text: "Estado", value: "estado", sortable: false },
        { text: "Opciones", value: "opciones", sortable: false },
      ],
      cabeceraDetalles: [
        { text: "Código", value: "idEquipo", sortable: false },
        { text: "Categoría", value: "categoria", sortable: false },
        { text: "Marca", value: "marca", sortable: false },
        { text: "Modelo", value: "modelo", sortable: false },
        { text: "Serie", value: "serie", sortable: false },
        { text: "Código Activo", value: "codigoActivo", sortable: false },
        { text: "Borrar", value: "borrar", sortable: false },
      ],
      cabeceraEquipos: [
        { text: "Categoria", value: "categoria", sortable: false },
        { text: "Marca", value: "marca", sortable: false },
        { text: "Modelo", value: "modelo", sortable: false },
        { text: "Serie", value: "serie" },
        { text: "Código Activo", value: "codigoActivo" },
        { text: "Seleccionar", value: "seleccionar", sortable: false },
      ],
      comprobantes: ["FACTURA", "RECIBO"],
      paginas: "Ingresos por Página",
      pagina: "Equipos por Página",
      search: "",
      editedIndex: -1,
      id: "",
      idproveedor: "",
      idusuario: "",
      texto: "",
      fechaingreso: "",
      numeroingreso: "",
      tipocomprobante: "",
      numerocomprobante: "",
      numeroorden: "",
      autorizado: "",
      observaciones: "",
      serie: "",
      validaMensaje: [],
      errorEquipo: null,
      verDet: 0,
      verNuevo: 0,
      verEquipos: 0,
      adModal: 0,
      adAccion: 0,
      adIngreso: "",
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
          load (val) {
        if (!val) return
        setTimeout(() => (this.load = false), 4000)
      },
  },

  created() {
    this.listar();
    this.seleccionarProveedor();
  },
  methods: {
    mostrarNuevo() {
      this.verNuevo = 1;
    },
    ocultarNuevo() {
      this.verNuevo = 0;
      this.limpiar();
    },
    uppercase() {
      this.autorizado = this.autorizado.toUpperCase();
      this.observaciones = this.observaciones.toUpperCase();
      this.serie = this.serie.toUpperCase();
      this.numeroorden = this.numeroorden.toUpperCase();
    },
    buscarSerie() {
      let me = this;
      me.errorEquipo = null;
      let header = { Authorization: "Bearer " + this.$store.state.token };
      let configuracion = { headers: header };
      axios
        .get("api/Equipos/BuscarSerie/" + this.serie, configuracion)
        .then(function(response) {
          me.agregarDetalle(response.data);
        })
        .catch(function(error) {
          if (error.response.status == 401) {
            me.$store.dispatch("salir");
          } else {
            console.log(error);
            me.errorEquipo = "No existe el equipo";
          }
        });
    },
    buscarEquipo() {
      let me = this;
      let header = { Authorization: "Bearer " + this.$store.state.token };
      let configuracion = { headers: header };
      axios
        .get("api/Equipos/EquipoIngreso/" + me.texto, configuracion)
        .then(function(response) {
          me.equipos = response.data;
        })
        .catch(function(error) {
          if (error.response.status == 401) {
            me.$store.dispatch("salir");
          } else {
            console.log(error);
          }
        });
    },
    mostrarEquipos() {
      this.verEquipos = 1;
    },
    ocultarEquipos() {
      this.verEquipos = 0;
      this.texto = "";
      this.equipos = [];
    },
    agregarDetalle(data = []) {
      this.errorEquipo = null;
      if (this.encuentra(data["idEquipo"])) {
        this.errorEquipo = "El equipo ya ha sido agregado";
      } else {
        this.detalles.push({
          idEquipo: data["idEquipo"],
          categoria: data["categoria"],
          marca: data["marca"],
          modelo: data["modelo"],
          serie: data["serie"],
          codigoActivo: data["codigoActivo"]
        });
      }
    },
    eliminarDetalle(arr, item) {
      var i = arr.indexOf(item);
      if (i !== -1) {
        arr.splice(i, 1);
      }
    },
    encuentra(id) {
      var sw = 0;
      for (var i = 0; i < this.detalles.length; i++) {
        if (this.detalles[i].idEquipo == id) {
          sw = 1;
        }
      }
      return sw;
    },
    listar() {
      let me = this;
      let header = { Authorization: "Bearer " + this.$store.state.token };
      let configuracion = { headers: header };
      axios
        .get("api/Ingresos/Listar", configuracion)
        .then(function(response) {
          me.ingresos = response.data;
        })
        .catch(function(error) {
          if (error.response.status == 401) {
            me.$store.dispatch("salir");
          } else {
            console.log(error);
          }
        });
    },
    listarDetalles(id) {
      let me = this;
      let header = { Authorization: "Bearer " + this.$store.state.token };
      let configuracion = { headers: header };
      axios
        .get("api/Ingresos/ListarDetalles/" + id, configuracion)
        .then(function(response) {
          me.detalles = response.data;
        })
        .catch(function(error) {
          if (error.response.status == 401) {
            me.$store.dispatch("salir");
          } else {
            console.log(error);
          }
        });
    },
    verDetalles(item) {
      this.limpiar();
      this.tipocomprobante = item.tipoComprobante;
      this.numerocomprobante = item.numeroComprobante;
      this.numeroorden = item.numeroOrden;
      this.numeroingreso = item.numeroIngreso;
      this.idproveedor = item.idProveedor;
      this.autorizado = item.autorizado;
      this.observaciones = item.observaciones;
      this.listarDetalles(item.idIngreso);
      this.verNuevo = 1;
      this.verDet = 1;
    },
    seleccionarProveedor() {
      let me = this;
      let header = { Authorization: "Bearer " + this.$store.state.token };
      let configuracion = { headers: header };
      var proveedoresArray = [];
      axios
        .get("api/Proveedores/Seleccionar", configuracion)
        .then(function(response) {
          proveedoresArray = response.data;
          proveedoresArray.map(function(x) {
            me.proveedores.push({ text: x.razonSocial, value: x.idProveedor });
          });
        })
        .catch(function(error) {
          if (error.response.status == 401) {
            me.$store.dispatch("salir");
          } else {
            console.log(error);
          }
        });
    },
    limpiar() {
      this.id = "";
      this.idproveedor = "";
      this.tipocomprobante = "";
      this.numerocomprobante = "";
      this.numeroingreso = "";
      this.numeroorden = "";
      this.autorizado = "";
      this.observaciones = "";
      this.serie = "";
      this.detalles = [];
      this.verDet = 0;
    },
    guardar() {
      if (this.validar()) {
        return;
      }
      if (this.$refs.form.validate()) {
        let header = { Authorization: "Bearer " + this.$store.state.token };
        let configuracion = { headers: header };
        let me = this;
        axios
          .post(
            "api/Ingresos/Crear",
            {
              idProveedor: me.idproveedor,
              idUsuario: me.$store.state.usuario.idusuario,
              tipoComprobante: me.tipocomprobante,
              numeroComprobante: me.numerocomprobante,
              numeroOrden: me.numeroorden,
              autorizado: me.autorizado,
              observaciones: me.observaciones,
              detalles: me.detalles,
            },
            configuracion
          )
          .then(function(response) {
            me.ocultarNuevo();
            me.listar();
            me.limpiar();
          })
          .catch(function(error) {
            console.log(error);
          });
      }
    },
    activardesactivarMostrar(accion, item) {
      (this.adModal = 1), (this.adIngreso = item.numeroIngreso);
      this.adId = item.idIngreso;
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
    desactivar() {
      let me = this;
      let header = { Authorization: "Bearer " + this.$store.state.token };
      let configuracion = { headers: header };
      axios
        .put("api/Ingresos/Anular/" + this.adId, {}, configuracion)
        .then(function(response) {
          me.adModal = 0;
          me.adAccion = 0;
          me.adIngreso = "";
          me.adId = "";
          me.listar();
        })
        .catch(function(error) {
          if (error.response.status == 401) {
            me.$store.dispatch("salir");
          } else {
            console.log(error);
          }
        });
    },
    validar() {
      this.valida = 0;
      this.validaMensaje = [];
      if (this.detalles.length <= 0) {
        this.validaMensaje.push("Ingrese al menos un equipo al detalle!");
      }
      return this.valida;
    },
  },
};
</script>
