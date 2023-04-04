<template>
  <div id="app">

  <RetrospectivesView/>
</div>
</template>

<script>
import RetrospectivesView from '@/views/RetrospectivesView'

export default {
  name: 'App',
  components: {
    RetrospectivesView
  }
}
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  text-align: center;
}
</style>
