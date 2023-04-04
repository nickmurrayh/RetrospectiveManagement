<template>
    <div v-if="visible" class="modal fade show" tabindex="-1"  role="dialog" style="display:block">
  <div class="modal-dialog modal-dialog-centered">
    <div class="modal-content">
      <div class="modal-header">New retrospective
        <button type="button" class="btn-close" @click="handleClose()"></button>
      </div>
      <div class="modal-body">
        <div class="container">
        <form @submit.prevent="handleSubmit">
            <div class="form-field">
                <label for="name" class="form-label">Name</label>
                <input type="text" class="form-control" id="name" v-model="form.name" />
            </div>
            <div class="form-field">
                <label for="date" class="form-label">Date</label>
                <input type="date" class="form-control" id="date" v-model="form.date" />
            </div>
            <div class="form-field">
                <label for="summary" class="form-label">Summary</label>
                <textarea class="form-control" id="summary" rows="3" v-model="form.summary"></textarea>
            </div>
           
            <div class="form-field">
                <label for="participants" class="form-label">Participants</label>
                <input type="text" class="form-control" id="participants" v-model="participantsInput"/>
            </div>
            <div class="text-end">
                <button type="submit" class="btn btn-primary">Submit</button>
            </div>
            
        </form>
    </div>
      </div>
    </div>
  </div>
</div>
</template>

<script>
import { defineComponent, ref, computed } from "vue"

export default defineComponent({

    components:{
    },

    props:{
        visible: Boolean
    },

    setup(_,{emit}) {


        const handleClose = () => {
            emit("close")
        }

        

        const form = ref({
            name:'',
            summary:'',
            date:'',
            participants: []

        })

        const participantsInput = computed({
            get: () => form.value.participants.join(","),
            set: (value) => {
                form.value.participants = value.split(",").map((participant) => participant.trim())
            }
        })

        const validateForm = () => {
            if(!form.value.name){
                alert("Name is required")
                return false
            }
            if(form.value.participants.length === 0){
                alert("At least one participant is required")
                return false
            }
            return true
        }

        const handleSubmit = () => {
            if(validateForm()){
                emit("submitted",form.value)

            }
            else{
                console.error("Invalid form entry")
            }
        }

        return{
            form,
            participantsInput,
            validateForm,
            handleSubmit,
            handleClose
        }

    },
})
</script>

<style scoped>
.form-field{
    text-align: left;
    margin-bottom: 10px;
}
</style>