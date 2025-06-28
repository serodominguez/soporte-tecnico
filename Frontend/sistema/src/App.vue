<template>
  <v-app id="inspire">
    <v-navigation-drawer
      :clipped="this.$vuetify.breakpoint.lgAndUp"
      v-model="drawer"
      v-if="logueado"
      fixed
      app
    >
      <v-list dense>
        <template
          v-if="
            esAdministrador ||
            esJefeSistemas ||
            esSoporteFabrica ||
            esSoporteTienda ||
            esActivoFijo
          "
        >
          <v-list-tile :to="{ name: 'home' }">
            <v-list-tile-action>
              <v-icon>home</v-icon>
            </v-list-tile-action>
            <v-list-tile-title>Inicio</v-list-tile-title>
          </v-list-tile>
        </template>
        <template
          v-if="
            esAdministrador ||
            esJefeSistemas ||
            esSoporteFabrica ||
            esSoporteTienda
          "
        >
          <v-list-group>
            <v-list-tile slot="activator">
              <v-list-tile-content>
                <v-list-tile-title>Almacén</v-list-tile-title>
              </v-list-tile-content>
            </v-list-tile>
            <v-list-tile :to="{ name: 'ingresos' }">
              <v-list-tile-action>
                <v-icon>add_shopping_cart</v-icon>
              </v-list-tile-action>
              <v-list-tile-content>
                <v-list-tile-title>Ingresos</v-list-tile-title>
              </v-list-tile-content>
            </v-list-tile>
             <v-list-tile :to="{ name: 'entregas' }">
              <v-list-tile-action>
                <v-icon>add_to_queue</v-icon>
              </v-list-tile-action>
              <v-list-tile-content>
                <v-list-tile-title>Entregas</v-list-tile-title>
              </v-list-tile-content>
            </v-list-tile>
            <v-list-tile :to="{ name: 'devoluciones' }">
              <v-list-tile-action>
                <v-icon>remove_from_queue</v-icon>
              </v-list-tile-action>
              <v-list-tile-content>
                <v-list-tile-title>Devoluciones</v-list-tile-title>
              </v-list-tile-content>
            </v-list-tile>
          </v-list-group>
        </template>
        <template
          v-if="
            esAdministrador ||
            esJefeSistemas ||
            esSoporteFabrica ||
            esSoporteTienda
          "
        >
          <v-list-group>
            <v-list-tile slot="activator">
              <v-list-tile-content>
                <v-list-tile-title>Empresa</v-list-tile-title>
              </v-list-tile-content>
            </v-list-tile>
            <v-list-tile :to="{ name: 'areas' }">
              <v-list-tile-action>
                <v-icon>maps_home_work</v-icon>
              </v-list-tile-action>
              <v-list-tile-content>
                <v-list-tile-title>Areas</v-list-tile-title>
              </v-list-tile-content>
            </v-list-tile>
            <v-list-tile :to="{ name: 'consignatarios' }">
              <v-list-tile-action>
                <v-icon>portrait</v-icon>
              </v-list-tile-action>
              <v-list-tile-content>
                <v-list-tile-title>Consignatarios</v-list-tile-title>
              </v-list-tile-content>
            </v-list-tile>
            <v-list-tile :to="{ name: 'personales' }">
              <v-list-tile-action>
                <v-icon>contact_page</v-icon>
              </v-list-tile-action>
              <v-list-tile-content>
                <v-list-tile-title>Personal</v-list-tile-title>
              </v-list-tile-content>
            </v-list-tile>
            <v-list-tile :to="{ name: 'proveedores' }">
              <v-list-tile-action>
                <v-icon>contact_phone</v-icon>
              </v-list-tile-action>
              <v-list-tile-content>
                <v-list-tile-title>Proveedores</v-list-tile-title>
              </v-list-tile-content>
            </v-list-tile>
           <v-list-tile :to="{ name: 'secciones' }">
              <v-list-tile-action>
                <v-icon>store</v-icon>
              </v-list-tile-action>
              <v-list-tile-content>
                <v-list-tile-title>Secciones</v-list-tile-title>
              </v-list-tile-content>
            </v-list-tile>
            <v-list-tile :to="{ name: 'tiendas' }">
              <v-list-tile-action>
                <v-icon>storefront</v-icon>
              </v-list-tile-action>
              <v-list-tile-content>
                <v-list-tile-title>Tiendas</v-list-tile-title>
              </v-list-tile-content>
            </v-list-tile>
          </v-list-group>
        </template>
        <template
          v-if="
            esAdministrador ||
            esJefeSistemas ||
            esSoporteFabrica ||
            esSoporteTienda
          "
        >
          <v-list-group>
            <v-list-tile slot="activator">
              <v-list-tile-content>
                <v-list-tile-title>Dispositivo</v-list-tile-title>
              </v-list-tile-content>
            </v-list-tile>
            <v-list-tile :to="{ name: 'accesorios' }">
              <v-list-tile-action>
                <v-icon>keyboard</v-icon>
              </v-list-tile-action>
              <v-list-tile-content>
                <v-list-tile-title>Accesorios</v-list-tile-title>
              </v-list-tile-content>
            </v-list-tile>
            <v-list-tile :to="{ name: 'categorias' }">
              <v-list-tile-action>
                <v-icon>widgets</v-icon>
              </v-list-tile-action>
              <v-list-tile-content>
                <v-list-tile-title>Categorias</v-list-tile-title>
              </v-list-tile-content>
            </v-list-tile>
            <v-list-tile :to="{ name: 'equipos' }">
              <v-list-tile-action>
                <v-icon>devices</v-icon>
              </v-list-tile-action>
              <v-list-tile-content>
                <v-list-tile-title>Equipos</v-list-tile-title>
              </v-list-tile-content>
            </v-list-tile>
            <v-list-tile :to="{ name: 'licencias' }">
              <v-list-tile-action>
                <v-icon>cloud_circle</v-icon>
              </v-list-tile-action>
              <v-list-tile-content>
                <v-list-tile-title>Licencias</v-list-tile-title>
              </v-list-tile-content>
            </v-list-tile>
            <v-list-tile :to="{ name: 'marcas' }">
              <v-list-tile-action>
                <v-icon>copyright</v-icon>
              </v-list-tile-action>
              <v-list-tile-content>
                <v-list-tile-title>Marcas</v-list-tile-title>
              </v-list-tile-content>
            </v-list-tile>
          </v-list-group>
        </template>
        <template v-if="esAdministrador || esJefeSistemas">
          <v-list-group>
            <v-list-tile slot="activator">
              <v-list-tile-content>
                <v-list-tile-title>Accesos</v-list-tile-title>
              </v-list-tile-content>
            </v-list-tile>
            <v-list-tile :to="{ name: 'usuarios' }">
              <v-list-tile-action>
                <v-icon>group</v-icon>
              </v-list-tile-action>
              <v-list-tile-content>
                <v-list-tile-title>Usuarios</v-list-tile-title>
              </v-list-tile-content>
            </v-list-tile>
            <v-list-tile :to="{ name: 'roles' }">
              <v-list-tile-action>
                <v-icon>manage_accounts</v-icon>
              </v-list-tile-action>
              <v-list-tile-content>
                <v-list-tile-title>Roles</v-list-tile-title>
              </v-list-tile-content>
            </v-list-tile>
          </v-list-group>
        </template>
        <template
          v-if="
            esAdministrador ||
            esJefeSistemas ||
            esSoporteFabrica ||
            esSoporteTienda ||
            esActivoFijo
          "
        >
          <v-list-group>
            <v-list-tile slot="activator">
              <v-list-tile-content>
                <v-list-tile-title>Consultas</v-list-tile-title>
              </v-list-tile-content>
            </v-list-tile>
            <v-list-tile :to="{ name: '' }">
              <v-list-tile-action>
                <v-icon>analytics</v-icon>
              </v-list-tile-action>
              <v-list-tile-content>
                <v-list-tile-title>Consultas entregas</v-list-tile-title>
              </v-list-tile-content>
            </v-list-tile>
            <v-list-tile :to="{ name: '' }">
              <v-list-tile-action>
                <v-icon>assessment</v-icon>
              </v-list-tile-action>
              <v-list-tile-content>
                <v-list-tile-title>Consultas devoluciones</v-list-tile-title>
              </v-list-tile-content>
            </v-list-tile>
          </v-list-group>
        </template>
      </v-list>
    </v-navigation-drawer>
    <v-toolbar
      color="blue darken-3"
      dark
      app
      :clipped-left="$vuetify.breakpoint.mdAndUp"
      fixed
    >
      <v-toolbar-title style="width: 300px" class="ml-0 pl-3">
        <v-toolbar-side-icon
          v-if="logueado"
          @click.stop="drawer = !drawer"
        ></v-toolbar-side-icon>
        <span v-if="logueado" class="hidden-sm-and-down">IT Inventarios</span>
      </v-toolbar-title>
      <v-spacer></v-spacer>
      <span v-if="logueado"
        ><strong>Usuario: {{ this.$store.state.usuario.usuario }}</strong></span
      >
      <v-btn @click="salir" v-if="logueado" icon>
        <v-icon>logout</v-icon>
      </v-btn>
    </v-toolbar>
    <v-content>
      <v-container fluid fill-height>
        <v-slide-y-transition mode="out-in">
          <router-view />
        </v-slide-y-transition>
      </v-container>
    </v-content>
    <v-footer dark height="auto">
      <v-layout justify-center>
        <v-flex text-xs-center>
          <v-card flat tile color="primary" class="white--text">
            <v-card-text class="white--text pt-0"
              ><strong
                >COPYRIGHT V 1.0.1-{{ new Date().getFullYear() }}</strong
              ></v-card-text
            >
          </v-card>
        </v-flex>
      </v-layout>
    </v-footer>
  </v-app>
</template>

<script>
export default {
  name: "App",
  data() {
    return {
      drawer: null,
    };
  },
  computed: {
    logueado() {
      return this.$store.state.usuario;
    },
    esAdministrador() {
      return (
        this.$store.state.usuario &&
        this.$store.state.usuario.rol == "Administrador"
      );
    },
    esJefeSistemas() {
      return (
        this.$store.state.usuario &&
        this.$store.state.usuario.rol == "Jefe de Sistemas"
      );
    },
    esSoporteFabrica() {
      return (
        this.$store.state.usuario &&
        this.$store.state.usuario.rol == "Soporte Fabrica"
      );
    },
    esSoporteTienda() {
      return (
        this.$store.state.usuario &&
        this.$store.state.usuario.rol == "Soporte Tienda"
      );
    },
    esActivoFijo() {
      return (
        this.$store.state.usuario &&
        this.$store.state.usuario.rol == "Activos Fijos"
      );
    },
  },
  created() {
    this.$store.dispatch("autoLogin");
  },
  methods: {
    salir() {
      this.$store.dispatch("salir");
    },
  },
};
</script>
