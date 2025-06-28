<template>
  <v-layout align-start>
    <v-flex>
      <v-toolbar flat color="white">
        <v-toolbar-title>Devoluciones {{ this.numeroentrega }} </v-toolbar-title>
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
<!--         <v-btn
          v-if="verNuevo == 0"
          @click="mostrarNuevo"
          color="primary"
          dark
          class="mb-2"
          >Nuevo</v-btn
        > -->
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
                          <td>{{ props.item.modelo }}</td>
                          <td>{{ props.item.serie }}</td>
                          <td>{{ props.item.marca }}</td>
                          <td>{{ props.item.categoria }}</td>
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
                >Cancelar</v-btn
              >
            </v-card-actions>
          </v-card>
        </v-dialog>
        <v-dialog v-model="entregaModal" max-width="1000px">
          <v-card>
            <v-card-text>
              <v-btn @click="crearPDF()"><v-icon>print</v-icon></v-btn>
              <div id="entrega">
                <header>
                  <div id="logo">
                    <img
                      src="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAKgAAAEsCAMAAABgwwj8AAAAP1BMVEX9/f39/vn8/f39/fv9/f/5/vz9/vr0/f74/f3y/v71/f/2/v3t/v3w/v7z/vz2//v++vP5+PXo/v/r/P//+fR8p2CTAAABIUlEQVR4nO3Yy27CMBBAUezYsfMCGvr/31pAqmCBBJs2sXTOMqurcTxScjgAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAMBfSVsHfCTnGrdu+ESajm2ExlLq1g0fObUSOg5jaeLor6G1hVsfx6E0EBpTHsYpb53xXkz12MA8r0s0nhpZ94fYwLnftXDuAAAA8G9SjDmXKYe9fzHHPJYynPrQbV3yRqzLMq/5HPrfJ13f7y86xrHM6+XydX60dWF/0035OMzzupbz3kPTuCzrfDl9P+JC6EPYtOqFVMsyL0MJT0MMN1tGvRLrVOtxys93/jrPx8XajXj/gdt1zy9lv8POdIjptujT07ZP1+q9L38AAIDW/ABivwT1SNSFJgAAAABJRU5ErkJggg=="
                      id="imagen"
                    />
                  </div>
                  <div id="datos">
                    <p id="encabezado">
                      <b>MANACO</b><br />Av. Thomas Bata, Quillacollo -
                      Cochabamba, Bolivia<br />Teléfono: 4262900<br />Web:
                      www.manaco.com.bo
                    </p>
                  </div>
                  <div id="ent">
                    <p>DEVOLUCION<br>
                      Número Entrega: {{ numeroentrega }}<br>
                      Número Devolución: {{ numerodevolucion }}<br>
                      Fecha Entrega: {{ fechaentrega }}<br>
                      Fecha Devolución: {{ fechadevolucion }}</p>
                  </div>
                </header>
                <br />
                <section>
                  <div>
                    <table id="enempleado">
                      <tbody>
                        <tr>
                          <td id="empleado">
                            <strong>Sr(a).</strong> {{ personal }}<br />
                            <strong>Código:</strong> {{ pkempleado }}<br />
                            <strong>Cargo:</strong> {{ cargo }}<br />
                            <strong>Celular:</strong> {{ celular }}<br />
                          </td>
                        </tr>
                      </tbody>
                    </table>
                  </div>
                </section>
                <br />
                <section>
                  <div>
                    <table id="entequipo">
                      <thead>
                        <tr id="en">
                          <th>CATEGORIA</th>
                          <th>MARCA</th>
                          <th>MODELO</th>
                          <th>SERIE</th>
                          <th>CODIGO ACTIVO</th>
                        </tr>
                      </thead>
                      <tbody>
                        <tr v-for="det in detalles" :key="det.iddetalleDocumentos">
                          <td style="text-align:center;">{{ det.categoria }}</td>
                          <td style="text-align:center;">{{ det.marca }}</td>
                          <td style="text-align:center;">{{ det.modelo }}</td>
                          <td style="text-align:center;">{{ det.serie }}</td>
                          <td style="text-align:center;">{{ det.codigoActivo }}</td>
                        </tr>
                      </tbody>
                    </table>
                  </div>
                </section>
                <br />
                <br />
                <td><strong>Observaciones:</strong> {{ observaciones }}</td>
                <br />
                <br />
                <footer>
                  <div id="firmas">
                    <p>
                      <b>Recibido por: &nbsp;</b>
                      &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                      &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                      &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                      &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                      <b>Entregado por: </b>
                      &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                      &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                      &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                      &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                      <b>Activos Fijos: </b>
                    </p>
                  </div>
                </footer>
              </div>
              <v-btn @click="ocultarEntrega()" color="red" flat
                >Cancelar</v-btn
              >
            </v-card-text>
          </v-card>
        </v-dialog>
      </v-toolbar>
      <v-data-table
        :headers="headers"
        :items="devoluciones"
        :search="search"
        :rows-per-page-text="paginas"
        class="elevation-1"
        v-if="verNuevo == 0"
      >
        <template slot="items" slot-scope="props">
          <td>{{ props.item.usuario }}</td>
          <td>{{ props.item.personal }}</td>
          <td>{{ props.item.seccion }}</td>
          <td>{{ props.item.numeroEntrega }}</td>
          <td>{{ props.item.numeroDevolucion }}</td>
          <td>{{ props.item.fechaEntrega }}</td>
          <td>{{ props.item.fechaDevolucion }}</td>
          <td>
            <div v-if="props.item.estado == 'Entregado'">
              <span class="green--text">{{ props.item.estado }}</span>
            </div>
            <div v-else>
              <span class="red--text">{{ props.item.estado }}</span>
            </div>
          </td>
          <td v-if="props.item.estado == 'Entregado'"> 
            <v-icon small class="mr-2" @click="verDetalles(props.item)"
              >block</v-icon
            >
          </td>
          <td v-else> 
            <v-icon small class="mr-2" @click="mostrarEntrega(props.item)"
              >print</v-icon
            >
          </td>
        </template>
        <template slot="no-data">
          <v-btn color="primary" @click="listar">Resetear</v-btn>
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
            <v-flex xs12 sm12 md4 lg4 xl4>
              <v-autocomplete
                v-model="idpersonal"
                :rules="[rules.requerido]"
                :items="personales"
                label="Personal"
                readonly
              ></v-autocomplete>
            </v-flex>
            <v-flex xs12 sm12 md4 lg4 xl4>
              <v-autocomplete
                v-model="idseccion"
                :items="secciones"
                :rules="[rules.requerido]"
                label="Sección"
                no-data-text="No hay datos disponibles"
                readonly
              ></v-autocomplete>
            </v-flex>
            <v-flex xs12 sm12 md4 lg4 xl4>
                    <v-text-field
                      v-model="observaciones"
                      @keyup="uppercase"
                      maxlength="100"
                      label="Observaciones"
                    ></v-text-field>
            </v-flex>
<!--             <v-flex xs12 sm2 md2 lg2 xl2>
              <v-btn @click="mostrarEquipos()" fab dark color="teal">
                <v-icon dark>list</v-icon>
              </v-btn>
            </v-flex> -->
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
<!--                   <td>
                    <v-icon
                      small
                      class="mr-2"
                      @click="eliminarDetalle(detalles, props.item)"
                      >delete</v-icon
                    >
                  </td> -->
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
                <v-btn  @click="guardar()" color="success"
                  >Guardar</v-btn
                >
                <v-btn @click="ocultarNuevo()" color="error"
                  >Cancelar</v-btn
                >
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
import jsPDF from "jspdf";
import html2canvas from "html2canvas";
export default {
  data() {
    return {
      devoluciones: [],
      secciones: [],
      personales: [],
      detalles: [],
      equipos: [],
      dialog: false,
      headers: [
        { text: "Usuario", value: "usuario" },
        { text: "Personal", value: "personal" },
        { text: "Sección", value: "seccion" },
        { text: "Número de Entrega", value: "numeroEntrega", sortable: false },
        { text: "Número de Devolución", value: "numeroDevolucion", sortable: false },
        { text: "Fecha de Entrega", value: "fechaEntrega", sortable: false },
        { text: "Fecha de Devolución", value: "fechaDevolucion", sortable: false },
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
      ],
      cabeceraEquipos: [
        { text: "Modelo", value: "modelo" },
        { text: "Serie", value: "serie" },
        { text: "Marca", value: "marca", sortable: false },
        { text: "Categoria", value: "categoria", sortable: false },
        { text: "Seleccionar", value: "seleccionar", sortable: false },
      ],
      paginas: "Devoluciones por Página",
      pagina: "Equipos por Página",
      search: "",
      editedIndex: -1,
      id: "",
      idpersonal: "",
      idusuario: "",
      idseccion: "",
      texto: "",
      numeroentrega: "",
      numerodevolucion: "",
      fechaentrega: "",
      fechadevolucion: "",
      observaciones: "",
      serie: "",
      estado: "",
      personal: "",
      pkempleado: "",
      celular: "",
      cargo: "",
      validaMensaje: [],
      errorEquipo: null,
      verDet: 0,
      verNuevo: 0,
      verEquipos: 0,
      adModal: 0,
      adAccion: 0,
      adEntrega: "",
      adId: "",
      error: null,
      entregaModal: 0,
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
    this.seleccionarPersonal();
    this.seleccionarSeccion();
  },
  methods: {
    crearPDF(){
      var quotes = document.getElementById('entrega')
      html2canvas(quotes).then(function(canvas) {
        var imgData = canvas.toDataURL('image/png')
        var imgWidth = 210;
        var pageHeight = 295;
        var imgHeight = canvas.height * imgWidth / canvas.width;
        var heightLeft = imgHeight;
        var doc = new jsPDF('p', 'mm', 'letter')
        var position = 0;
        doc.setProperties({
          title: "Nota de Devolución"
         });
        doc.addImage(imgData, 'PNG', 0, position, imgWidth, imgHeight);
        window.open(doc.output('bloburl'));
         //doc.output('dataurlnewwindow');
      });
    },
    mostrarEntrega(item){
      this.limpiar();
      this.numeroentrega = item.numeroEntrega;
      this.numerodevolucion = item.numeroDevolucion;
      this.fechaentrega = item.fechaEntrega;
      this.fechadevolucion = item.fechaDevolucion;
      this.estado = item.estado;
      this.personal = item.personal;
      this.pkempleado = item.pkEmpleado;
      this.cargo = item.cargo;
      this.celular = item.celular;
      this.observaciones = item.observaciones;
      this.listarDetalles(item.idDocumento);
      this.entregaModal = 1;
    },
    ocultarEntrega(){
      this.entregaModal = 0;
      this.limpiar();
    },
    mostrarNuevo() {
      this.verNuevo = 1;
    },
    ocultarNuevo() {
      this.verNuevo = 0;
      this.limpiar();
    },
    uppercase() {
      this.serie = this.serie.toUpperCase();
    },
    buscarSerie() {
      let me = this;
      me.errorEquipo = null;
      let header = { Authorization: "Bearer " + this.$store.state.token };
      let configuracion = { headers: header };
      axios
        .get("api/Equipos/BuscarSerie/" + this.serie, configuracion)
        .then(function (response) {
          me.agregarDetalle(response.data);
        })
        .catch(function (error) {
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
        .get("api/Equipos/EquipoEntrega/" + me.texto, configuracion)
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
    mostrarEquipos() {
      this.verEquipos = 1;
    },
    ocultarEquipos() {
      this.verEquipos = 0;
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
        .get("api/Documentos/ListarDevoluciones", configuracion)
        .then(function (response) {
          me.devoluciones = response.data;
        })
        .catch(function (error) {
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
        .get("api/Documentos/ListarDetalles/" + id, configuracion)
        .then(function (response) {
          me.detalles = response.data;
        })
        .catch(function (error) {
          if (error.response.status == 401) {
            me.$store.dispatch("salir");
          } else {
            console.log(error);
          }
        });
    },
    verDetalles(item) {
      this.limpiar();
      this.id = item.idDocumento;
      this.numeroentrega = item.numeroEntrega;
      this.idpersonal = item.idPersonal;
      this.idseccion = item.idSeccion;
      this.listarDetalles(item.idDocumento);
      this.verNuevo = 1;
      this.verDet = 1;
    },
    seleccionarPersonal() {
      let me = this;
      let header = { Authorization: "Bearer " + this.$store.state.token };
      let configuracion = { headers: header };
      var personalesArray = [];
      axios
        .get("api/Personales/SeleccionarPersonal", configuracion)
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
    seleccionarSeccion() {
      let me = this;
      let header = { Authorization: "Bearer " + this.$store.state.token };
      let configuracion = { headers: header };
      var seccionesArray = [];
      axios
        .get("api/Secciones/SeleccionarSeccion", configuracion)
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
    limpiar() {
      this.id = "";
      this.idpersonal = "";
      this.idseccion = "";
      this.numeroentrega = "";
      this.numerodevolucion = "";
      this.serie = "";
      this.fechaentrega = "";
      this.fechadevolucion = "";
      this.observaciones = "";
      this.estado = "";
      this.personal = "";
      this.pkempleado = "";
      this.cargo = "";
      this.celular = "";
      this.detalles = [];
      this.verDet = 0;
    },
    guardar() {
        let header = { Authorization: "Bearer " + this.$store.state.token };
        let configuracion = { headers: header };
        let me = this;
        axios
          .put(
            "api/Documentos/Actualizar",
            {
              idDocumento: me.id,
              observaciones: me.observaciones,
              detalles: me.detalles,
            },
            configuracion
          )
          .then(function (response) {
            me.ocultarNuevo();
            me.listar();
            me.limpiar();
          })
          .catch(function (error) {
            console.log(error);
          });
    },
    activardesactivarMostrar(accion, item) {
      (this.adModal = 1), (this.adEntrega = item.numeroEntrega);
      this.adId = item.idDocumento;
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
        .put("api/Documentos/Anular/" + this.adId, {}, configuracion)
        .then(function (response) {
          me.adModal = 0;
          me.adAccion = 0;
          me.adEntrega = "";
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
<style>
    #entrega {
        padding: 20px;
        font-family: Arial, sans-serif;
        font-size: 16px ;
    }

    #logo {
        float: left;
        margin-left: 2%;
        margin-right: 2%;
    }
    #imagen {
        width: 100px;
    }

    #ent {
        font-size: 18px;
        font-weight: bold;
        text-align: center;
    }   

    #datos {
        float: left;
        margin-top: 0%;
        margin-left: 2%;
        margin-right: 2%;
        /*text-align: justify;*/
    }

    #encabezado {
        text-align: center;
        margin-left: 10px;
        margin-right: 10px;
        font-size: 16px;
    }

    section {
        clear: left;
    }

    #empleado {
        text-align: left;
    }

    #enempleado {
        width: 40%;
        border-collapse: collapse;
        border-spacing: 0;
        margin-bottom: 15px;
    }

    #en {
        color: #FFFFFF;
        font-size: 14px;
    }

    #entequipo {
        width: 100%;
        border-collapse: collapse;
        border-spacing: 0;
        padding: 20px;
        margin-bottom: 15px;
    }

    #entequipo thead {
        padding: 20px;
        background: #2183E3;
        text-align: center;
        border-bottom: 1px solid #FFFFFF;
    }

    #firmas {
        text-align: center;
    }
</style>

