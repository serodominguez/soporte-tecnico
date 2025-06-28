import Vue from 'vue'
import Router from 'vue-router'
import Home from './views/Home.vue'
import Accesorio from './components/Accesorio.vue'
import Area from './components/Area.vue'
import Categoria from './components/Categoria.vue'
import Consignatario from './components/Consignatario.vue'
import Devolucion from './components/Devolucion.vue'
import Entrega from './components/Entrega.vue'
import Equipo from './components/Equipo.vue'
import Licencia from './components/Licencia.vue'
import Ingreso from './components/Ingreso.vue'
import Inicio from './components/Inicio.vue'
import Marca from './components/Marca.vue'
import Personal from './components/Personal.vue'
import Proveedor from './components/Proveedor.vue'
import Rol from './components/Rol.vue'
import Seccion from './components/Seccion.vue'
import Tienda from './components/Tienda.vue'
import Usuario from './components/Usuario.vue'
import store from './store'

Vue.use(Router)

var router = new Router({
  mode: 'history',
  base: process.env.BASE_URL,
  routes: [
    {
      path: '/',
      name: 'home',
      component: Home,
      meta: {
        administrador: true,
        jefesistemas: true,
        soportefabrica: true,
        soportetienda: true,
        activosfijos: true
      }
    },
    {
      path: '/accesorios',
      name: 'accesorios',
      component: Accesorio,
      meta: {
        administrador: true,
        jefesistemas: true,
        soportefabrica: true,
        soportetienda: true
      }
    },
    {
      path: '/areas',
      name: 'areas',
      component: Area,
      meta: {
        administrador: true,
        jefesistemas: true,
        soportefabrica: true,
        soportetienda: true
      }
    },
    {
      path: '/categorias',
      name: 'categorias',
      component: Categoria,
      meta: {
        administrador: true,
        jefesistemas: true,
        soportefabrica: true,
        soportetienda: true
      }
    },
    {
      path: '/consignatarios',
      name: 'consignatarios',
      component: Consignatario,
      meta: {
        administrador: true,
        jefesistemas: true,
        soportetienda: true
      }
    },
    {
      path: '/devoluciones',
      name: 'devoluciones',
      component: Devolucion,
      meta: {
        administrador: true,
        jefesistemas: true,
        soportefabrica: true,
        soportetienda: true
      }
    },
    {
      path: '/entregas',
      name: 'entregas',
      component: Entrega,
      meta: {
        administrador: true,
        jefesistemas: true,
        soportefabrica: true,
        soportetienda: true
      }
    },
    {
      path: '/equipos',
      name: 'equipos',
      component: Equipo,
      meta: {
        administrador: true,
        jefesistemas: true,
        soportefabrica: true,
        soportetienda: true
      }
    },
    {
      path: '/licencias',
      name: 'licencias',
      component: Licencia,
      meta: {
        administrador: true,
        jefesistemas: true,
        soportefabrica: true,
        soportetienda: true
      }
    },
    {
      path: '/ingresos',
      name: 'ingresos',
      component: Ingreso,
      meta: {
        administrador: true,
        jefesistemas: true,
        soportefabrica: true,
        soportetienda: true
      }
    },
    {
      path: '/inicio',
      name: 'inicio',
      component: Inicio,
      meta: {
        libre: true
      }
    },
    {
      path: '/marcas',
      name: 'marcas',
      component: Marca,
      meta: {
        administrador: true,
        jefesistemas: true,
        soportefabrica: true,
        soportetienda: true
      }
    },
    {
      path: '/personales',
      name: 'personales',
      component: Personal,
      meta: {
        administrador: true,
        jefesistemas: true,
        soportefabrica: true
      }
    },
    {
      path: '/proveedores',
      name: 'proveedores',
      component: Proveedor,
      meta: {
        administrador: true,
        jefesistemas: true,
        soportefabrica: true,
        soportetienda: true
      }
    },
    {
      path: '/roles',
      name: 'roles',
      component: Rol,
      meta: {
        administrador: true,
        jefesistemas: true
      }
    },
    {
      path: '/secciones',
      name: 'secciones',
      component: Seccion,
      meta: {
        administrador: true,
        jefesistemas: true,
        soportefabrica: true
      }
    },
    {
      path: '/tiendas',
      name: 'tiendas',
      component: Tienda,
      meta: {
        administrador: true,
        jefesistemas: true,
        soportetienda: true
      }
    },
    {
      path: '/usuarios',
      name: 'usuarios',
      component: Usuario,
      meta: {
        administrador: true,
        jefesistemas: true
      }
    }
  ]
})
router.beforeEach((to, from, next) => {
  if(to.matched.some(record => record.meta.libre)){
    next()
  } else if (store.state.usuario && store.state.usuario.rol == 'Administrador'){
    if (to.matched.some(record => record.meta.administrador)){
        next()
    }
  } else if (store.state.usuario && store.state.usuario.rol == 'Jefe de Sistemas'){
    if (to.matched.some(record => record.meta.jefesistemas)){
        next()
    }
  } else if (store.state.usuario && store.state.usuario.rol == 'Soporte Fabrica'){
    if (to.matched.some(record => record.meta.soportefabrica)){
        next()
    }
  } else if (store.state.usuario && store.state.usuario.rol == 'Soporte Tienda'){
    if (to.matched.some(record => record.meta.soportetienda)){
        next()
    }
  } else if (store.state.usuario && store.state.usuario.rol == 'Activos Fijos'){
    if (to.matched.some(record => record.meta.activosfijos)){
        next()
    }
  } else {
    next({
      name: 'inicio'
    })
  }
})
export default router