<template>
<nav class="navbar bg-primary">
  <div class="container-fluid">
    <a class="navbar-brand text-light">{{ text }}</a>
    <form class="d-flex" role="search">
      <button type="button" class="btn btn-light" @click="$emit('onAction')">Create new</button>
    </form>
  </div>
</nav>
</template>

<script>
import { defineComponent } from "vue"



export default defineComponent({

  props:{
        text: String
  },
})
</script>
