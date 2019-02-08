import HomePage from 'components/home-page'
import Resources from 'components/resources/resource'

export const routes = [
  { name: 'home', path: '/', component: HomePage, display: 'Home', icon: 'home' },
  { name: 'resource', path: '/resource', component: Resources, display: 'Resources', icon: 'list' }
]
