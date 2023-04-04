<template>
    <div v-if="visible" class="modal fade show" tabindex="-1" role="dialog" style="display:block">
  <div class="modal-dialog modal-dialog-centered">
    <div class="modal-content">
      <div class="modal-header">{{retrospectiveName}} Feedback
        <button type="button" class="btn-close" @click="handleClose()"></button>
      </div>
      <div class="modal-body">
        <div class="container">
            <table class="table">
                <thead>
                    <tr>
                    <th scope="col">Created by</th>
                    <th scope="col">Body</th>
                    <th scope="col">Type</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="(feedbackItem, index) in feedbackItems" :key="index">
                    <td>{{ feedbackItem.createdBy }}</td>
                    <td>{{ feedbackItem.body }}</td>
                    <td>{{ feedbackItem.type }}</td>
                    </tr>
                </tbody>
            </table>
        </div>

        <div class="container">
        <form @submit.prevent="handleSubmit">
            <div class="form-field">
                <label for="name" class="form-label">Created by</label>
                <input type="text" class="form-control" id="createdBy" v-model="form.createdBy" />
            </div>
            <div class="form-field">
                <label for="date" class="form-label">Body</label>
                <textarea class="form-control" id="body" v-model="form.body"></textarea>
            </div>
            <div class="form-field">
                <label for="summary" class="form-label">Type</label>
                <select class="form-select" id="dropdown" v-model="form.type">
                    <option disabled value="">Select a type</option>
                    <option v-for="(option, index) in typeOptions" :key="index" :value="option.value">
                    {{ option.text }}
                    </option>
        </select>
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
import { defineComponent, ref } from "vue"

export default defineComponent({

    components:{
    },

    props:{
        visible: Boolean,
        retrospectiveName: String,
        feedbackItems: Array,
    },

    setup(props,{emit}) {

        

        const typeOptions = ref([
            {value: "positive", text: "Positive"},
            {value: "negative", text: "Negative"},
            {value: "idea", text: "Idea"},
            {value: "praise", text: "Praise"},
        ])
        const form = ref({
            createdBy:'',
            body:'',
            type:'',
            retrospectiveName: ''
        })

        const handleClose = () => {
            emit("close")
        }
        const handleSubmit = () => {
            if(validateForm()){
                form.value.retrospectiveName = props.retrospectiveName
                emit("submitted", form.value)
            }
        }
        const validateForm = () => {
            if(!form.value.createdBy) {
                alert("Created by is required")
                return false
            }

            if(!form.value.type){
                alert("Type is required")
                return false
            }
            return true
        }

        return{
            typeOptions,
            form,
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