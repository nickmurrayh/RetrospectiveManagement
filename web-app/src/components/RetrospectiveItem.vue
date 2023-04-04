<template>
  <div class="card item-container">
    <div class="card-header">
      <div class="text-end">{{ formatDate(item.date) ?? "Invalid date" }}</div>
      <h4 class="card-title">{{ item.name }}</h4>
      
    </div>
    <div class="card-body">
      <div class="row">
        <div class="col-4 text-left">
          <span><b>Participants:</b>
          </span>
          <ul>
            <li v-for="(participant, index) in item.participants" :key="index">{{ participant }}</li>
          </ul>
        </div>
        <div class="col-1">
          <div class="vertical-line"></div>
        </div>
        <div class="col-7">
          <p class="card-text">{{ item.summary && item.summary !== '' ? item.summary : 'No summary available' }}</p>
        </div>
      </div> 
    </div>
    <div class="card-footer">
      <div class="text-end">
        <button type="button" class="btn btn-primary " @click="handleViewFeedback()">View feedback</button>
      </div>
    </div>
  </div>
</template>

<script>
import { defineComponent } from "vue"

export default defineComponent({
  props:{
    item: {
      type: Object,
      required:true,
    } ,
  },

  setup(_,{emit}){
    const handleViewFeedback = () => {
      emit('onViewFeedback')
    }

    return{
      handleViewFeedback
    }
  
  }

})
</script>

<style scoped>
.item-container{
  min-height: 15rem;
  border-width: 2px;
  background-color: #FAFAFA;
  text-align: left;

}
.vertical-line{
  border-left: thin solid lightgray;
  height: 100%;
}
</style>