<template>
  <v-layout align-start>
    <v-flex>
      <v-toolbar flat color="white">
        <v-toolbar-title>Equipos</v-toolbar-title>
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
                  <v-flex xs12 sm12 md4 lg4 xl4>
                    <v-autocomplete
                      v-model="idmarca"
                      :items="marcas"
                      :rules="[rules.requerido]"
                      label="Marca"
                      no-data-text="No hay datos disponibles"
                    >
                    </v-autocomplete>
                  </v-flex>
                  <v-flex xs12 sm12 md4 lg4 xl4>
                    <v-autocomplete
                      v-model="idcategoria"
                      :items="categorias"
                      :rules="[rules.requerido]"
                      label="Categoría"
                      no-data-text="No hay datos disponibles"
                    >
                    </v-autocomplete>
                  </v-flex>
                  <v-flex xs12 sm12 md4 lg4 xl4>
                    <v-text-field
                      v-model="modelo"
                      :rules="[rules.requerido]"
                      @keyup="uppercase"
                      maxlength="30"
                      label="Modelo"
                    ></v-text-field>
                  </v-flex>
                  <v-flex xs12 sm12 md4 lg4 xl4>
                    <v-text-field
                      v-model="serie"
                      :rules="[rules.requerido]"
                      @keyup="uppercase"
                      maxlength="50"
                      label="Serie"
                    ></v-text-field>
                  </v-flex>
                  <v-flex xs12 sm12 md4 lg4 xl4>
                    <v-text-field
                      mask="##-##-####"
                      placeholder="Fecha de compra"
                      v-model="fechacompra"
                      @keyup="validarFecha"
                      :rules="[rules.requerido]"
                      return-masked-value
                    ></v-text-field>
                    <div
                      class="red--text font-weight-bold"
                      v-text="validaMensaje"
                    ></div>
                  </v-flex>
                  <v-flex xs12 sm12 md4 lg4 xl4>
                    <v-select
                      v-model="moneda"
                      :items="monedas"
                      :rules="[rules.requerido]"
                      label="Moneda"
                    ></v-select>
                  </v-flex>
                  <v-flex xs12 sm12 md4 lg4 xl4>
                    <v-text-field
                      v-model.number="preciocompra"
                      onkeypress="return (event.charCode >= 48 && event.charCode <= 57)"
                      maxlength="6"
                      label="Precio de compra"
                    ></v-text-field>
                  </v-flex>
                  <v-flex xs12 sm12 md4 lg4 xl4>
                    <v-text-field
                      v-model.number="mesesgarantia"
                      onkeypress="return (event.charCode >= 48 && event.charCode <= 57)"
                      maxlength="2"
                      label="Meses Garantía"
                    ></v-text-field>
                  </v-flex>
                  <v-flex xs12 sm12 md4 lg4 xl4>
                    <v-text-field
                      v-model="sistemaoperativo"
                      :rules="[rules.requerido]"
                      @keyup="uppercase"
                      maxlength="25"
                      label="Sistema Operativo"
                    ></v-text-field>
                  </v-flex>
                  <v-flex xs12 sm12 md4 lg4 xl4>
                    <v-text-field
                      v-model="codigoactivo"
                      @keyup="uppercase"
                      maxlength="15"
                      label="Código Activo Fijo"
                    ></v-text-field>
                  </v-flex>
                  <v-flex xs12 sm12 md4 lg4 xl4>
                    <v-text-field
                      v-model="nombreequipo"
                      @keyup="uppercase"
                      maxlength="20"
                      label="Nombre del equipo"
                    ></v-text-field>
                  </v-flex>
                  <v-flex xs12 sm12 md4 lg4 xl4>
                    <v-text-field
                      v-model="procesador"
                      :rules="[rules.requerido]"
                      @keyup="uppercase"
                      maxlength="20"
                      label="Procesador"
                    ></v-text-field>
                  </v-flex>
                  <v-flex xs12 sm12 md4 lg4 xl4>
                    <v-text-field
                      v-model="memoriaram"
                      :rules="[rules.requerido]"
                      @keyup="uppercase"
                      maxlength="15"
                      label="Memoria Ram"
                    ></v-text-field>
                  </v-flex>
                  <v-flex xs12 sm12 md4 lg4 xl4>
                    <v-text-field
                      v-model="almacenamiento"
                      :rules="[rules.requerido]"
                      @keyup="uppercase"
                      maxlength="15"
                      label="Almacenamiento"
                    ></v-text-field>
                  </v-flex>
                  <v-flex xs12 sm12 md4 lg4 xl4>
                    <v-text-field
                      v-model="tarjetavideo"
                      :rules="[rules.requerido]"
                      @keyup="uppercase"
                      maxlength="20"
                      label="Tarjeta Video"
                    ></v-text-field>
                  </v-flex>
                  <v-flex xs12 sm12 md4 lg12 xl12>
                    <v-select
                      v-model="condicion"
                      :items="condiciones"
                      :rules="[rules.requerido]"
                      label="Condición"
                    ></v-select>
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
              el ítem {{ adEquipo }}
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
        :items="equipos"
        :search="search"
        :rows-per-page-text="paginas"
        class="elevation-1"
      >
        <template slot="items" slot-scope="props">
          <td>{{ props.item.marca }}</td>
          <td>{{ props.item.categoria }}</td>
          <td>{{ props.item.modelo }}</td>
          <td>{{ props.item.serie }}</td>
          <td>{{ props.item.fechaCompra }}</td>
          <td>{{ props.item.precioCompra }}</td>
          <td>{{ props.item.moneda }}</td>
          <td>{{ props.item.mesesGarantia }}</td>
          <td>{{ props.item.sistemaOperativo }}</td>
          <td>{{ props.item.codigoActivo }}</td>
          <td>{{ props.item.nombreEquipo }}</td>
          <td>{{ props.item.procesador }}</td>
          <td>{{ props.item.memoriaRam }}</td>
          <td>{{ props.item.almacenamiento }}</td>
          <td>{{ props.item.tarjetaVideo }}</td>
          <td>{{ props.item.condicion }}</td>
          <td>{{ props.item.asignado }}</td>
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
            <!-- <template v-if="props.item.estado == 'Activo'">
              <v-icon small @click="activardesactivarMostrar(2, props.item)"
                >block</v-icon
              >
            </template>
            <template v-else>
              <v-icon small @click="activardesactivarMostrar(1, props.item)"
                >check</v-icon
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
    </v-flex>
  </v-layout>
</template>
<script>
import axios from "axios";
import * as moment from "moment";

export default {
  data() {
    return {
      equipos: [],
      marcas: [],
      categorias: [],
      dialog: false,
      headers: [
        { text: "Marcas", value: "marca" },
        { text: "Categorias", value: "categoria", sortable: false },
        { text: "Modelo", value: "modelo", sortable: false },
        { text: "Serie", value: "serie", sortable: false },
        { text: "Fecha de Compra", value: "fechaCompra", sortable: false },
        { text: "Precio de Compra", value: "precioCompra", sortable: false },
        { text: "Moneda", value: "moneda", sortable: false },
        { text: "Garantia", value: "mesesGarantia", sortable: false },
        { text: "Sistema Operativo", value: "sistemaOperativo", sortable: false },
        { text: "Código de Activo", value: "codigoActivo", sortable: false },
        { text: "Nombre de Equipo", value: "nombreEquipo", sortable: false },
        { text: "Procesador", value: "procesador", sortable: false },
        { text: "Memoria Ram", value: "memoriaRam", sortable: false },
        { text: "Almacenamiento", value: "almacenamiento", sortable: false },
        { text: "Tarjeta de Video", value: "tarjetaVideo", sortable: false },
        { text: "Condición", value: "condicion", sortable: false },
        { text: "Asignado", value: "asignado", sortable: false },
        { text: "Estado", value: "estado", sortable: false },
        { text: "Opciones", value: "opciones", sortable: false },
      ],
      condiciones: ["NUEVO", "USADO"],
      monedas: ["BOLIVIANOS"],
      paginas: "Equipos por Página",
      search: "",
      editedIndex: -1,
      id: "",
      idmarca: 0,
      idcategoria: 0,
      modelo: "",
      serie: "",
      fechacompra: "",
      preciocompra: "",
      mesesgarantia: 0,
      sistemaoperativo: "",
      codigoactivo: "",
      nombreequipo: "",
      procesador: "",
      memoriaram: "",
      almacenamiento: "",
      tarjetavideo: "",
      condicion: "",
      asignado: "",
      moneda: "",
      validaMensaje: "",
      adModal: 0,
      adAccion: 0,
      adEquipo: "",
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
      return this.editedIndex === -1 ? "Nuevo equipo" : "Actualizar equipo";
    },
  },

  watch: {
    dialog(val) {
      val || this.cerrar();
    },
    date(val) {
      this.dateFormatted = this.formatDate(this.date);
    },
  },
  created() {
    this.listar();
    this.seleccionarMarca();
    this.seleccionarCategoria();
  },

  methods: {
    uppercase() {
      this.modelo = this.modelo.toUpperCase();
      this.serie = this.serie.toUpperCase();
      this.sistemaoperativo = this.sistemaoperativo.toUpperCase();
      this.codigoactivo = this.codigoactivo.toUpperCase();
      this.nombreequipo = this.nombreequipo.toUpperCase();
      this.procesador = this.procesador.toUpperCase();
      this.memoriaram = this.memoriaram.toUpperCase();
      this.almacenamiento = this.almacenamiento.toUpperCase();
      this.tarjetavideo = this.tarjetavideo.toUpperCase();
    },
    validarFecha() {
      this.validaMensaje = "";
      var result = moment(this.fechacompra, "DD-MM-YYYY", true).isValid();
      if (result != true) {
        this.validaMensaje = "Fecha incorrecta!";
      }
    },
    listar() {
      let me = this;
      let header = { Authorization: "Bearer " + this.$store.state.token };
      let configuracion = { headers: header };
      axios
        .get("api/Equipos/ListarA", configuracion)
        .then(function (response) {
          me.equipos = response.data;
        })
        .catch(function (error) {
          if (error.response.status == 401) {
            me.$store.dispatch("salir");
          } else {
            console.log(error);
          }
        });
    },
    seleccionarMarca() {
      let me = this;
      let header = { Authorization: "Bearer " + this.$store.state.token };
      let configuracion = { headers: header };
      var marcasArray = [];
      axios
        .get("api/Marcas/Seleccionar", configuracion)
        .then(function (response) {
          marcasArray = response.data;
          marcasArray.map(function (x) {
            me.marcas.push({ text: x.marca, value: x.idMarca });
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
    seleccionarCategoria() {
      let me = this;
      let header = { Authorization: "Bearer " + this.$store.state.token };
      let configuracion = { headers: header };
      var categoriasArray = [];
      axios
        .get("api/Categorias/SeleccionarEquipos", configuracion)
        .then(function (response) {
          categoriasArray = response.data;
          categoriasArray.map(function (x) {
            me.categorias.push({ text: x.categoria, value: x.idCategoria });
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
      this.id = item.idEquipo;
      this.idmarca = item.idMarca;
      this.idcategoria = item.idCategoria;
      this.modelo = item.modelo;
      this.serie = item.serie;
      this.fechacompra = item.fechaCompra;
      this.preciocompra = item.precioCompra;
      this.moneda = item.moneda;
      this.mesesgarantia = item.mesesGarantia;
      this.sistemaoperativo = item.sistemaOperativo;
      this.codigoactivo = item.codigoActivo;
      this.nombreequipo = item.nombreEquipo;
      this.procesador = item.procesador;
      this.memoriaram = item.memoriaRam;
      this.almacenamiento = item.almacenamiento;
      this.tarjetavideo = item.tarjetaVideo;
      this.condicion = item.condicion;
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
      this.idmarca = 0;
      this.idcategoria = 0;
      this.modelo = "";
      this.serie = "";
      this.fechacompra = "";
      this.preciocompra = "";
      this.moneda = "";
      this.mesesgarantia = 0;
      this.sistemaoperativo = "";
      this.codigoactivo = "";
      this.nombreequipo = "";
      this.procesador = "";
      this.memoriaram = "";
      this.almacenamiento = "";
      this.tarjetavideo = "";
      this.condicion = "";
      this.editedIndex = -1;
      this.date = null;
      this.dateFormatted = null;
      this.menu = false;
      this.error = null;
      this.validaMensaje = "";
    },
    guardar() {
      var newfecha = this.fechacompra.split("-").reverse().join("-");
      if (this.$refs.form.validate()) {
        let header = { Authorization: "Bearer " + this.$store.state.token };
        let configuracion = { headers: header };
        if (this.editedIndex > -1) {
          this.error = null;
          let me = this;
          axios
            .put(
              "api/Equipos/ActualizarA",
              {
                idequipo: me.id,
                idmarca: me.idmarca,
                idcategoria: me.idcategoria,
                modelo: me.modelo,
                serie: me.serie,
                fechacompra: newfecha,
                preciocompra: me.preciocompra,
                mesesgarantia: me.mesesgarantia,
                sistemaoperativo: me.sistemaoperativo,
                codigoactivo: me.codigoactivo,
                nombreequipo: me.nombreequipo,
                procesador: me.procesador,
                memoriaram: me.memoriaram,
                almacenamiento: me.almacenamiento,
                tarjetavideo: me.tarjetavideo,
                condicion: me.condicion,
                moneda: me.moneda,
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
              "api/Equipos/CrearA",
              {
                idmarca: me.idmarca,
                idcategoria: me.idcategoria,
                modelo: me.modelo,
                serie: me.serie,
                cargo: me.cargo,
                fechacompra: newfecha,
                preciocompra: me.preciocompra,
                mesesgarantia: me.mesesgarantia,
                sistemaoperativo: me.sistemaoperativo,
                codigoactivo: me.codigoactivo,
                nombreequipo: me.nombreequipo,
                procesador: me.procesador,
                memoriaram: me.memoriaram,
                almacenamiento: me.almacenamiento,
                tarjetavideo: me.tarjetavideo,
                condicion: me.condicion,
                asignado: "No",
                estado: "Creado",
                moneda: me.moneda,
                tipo: 1
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
      (this.adModal = 1), (this.adEquipo = item.modelo);
      this.adId = item.idEquipo;
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
        .put("api/Equipos/Activar/" + this.adId, {}, configuracion)
        .then(function (response) {
          me.adModal = 0;
          me.adAccion = 0;
          me.adEquipo = "";
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
        .put("api/Equipos/Desactivar/" + this.adId, {}, configuracion)
        .then(function (response) {
          me.adModal = 0;
          me.adAccion = 0;
          me.adEquipo = "";
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
