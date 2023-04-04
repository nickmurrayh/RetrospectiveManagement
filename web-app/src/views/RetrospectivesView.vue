<template>
    <HeaderBar text="Retrospectives" @on-action="showAddRetrospectiveModal(true)"/>
    <div class="container-fluid grid-container ">
        <div class="row">
            <div class="col-3 mb-4" v-for="retrospective in retrospectiveItems" :key="retrospective.name" >
                <RetrospectiveItem 
                :item="retrospective" @onViewFeedback="viewRetrospectiveFeedback(retrospective.name, retrospective.feedbackItems)"/>
            </div>
        </div>
    </div>  
 
    <CreateRetrospective
     :visible="retrospectiveModalActive"
     @close="showAddRetrospectiveModal(false)" 
     @submitted="addNewRetrospective"/>

     <FeedbackView
     :visible="feedbackModalActive" 
     :retrospectiveName="selectedRetrospectiveName"
     :feedbackItems="selectedFeedbackItems"
     @close="showFeedbackModal(false)"
     @submitted="addNewRetrospectiveFeedback"/>

</template>

<script>

import RetrospectiveItem from '@/components/RetrospectiveItem'
import HeaderBar from '@/components/HeaderBar'
import CreateRetrospective from '@/components/CreateRetrospective.vue'
import FeedbackView from '@/components/FeedbackView.vue'
import { defineComponent, inject, ref } from 'vue'

export default defineComponent({
    inject:["retrospectiveApi"],

    components: {
      RetrospectiveItem,
      HeaderBar,
      CreateRetrospective,
      FeedbackView
    },

    setup(){
        const apiService = inject("retrospectiveApi")
        const retrospectiveItems = ref([]);
        const selectedFeedbackItems = ref([]);
        const selectedRetrospectiveName = ref('')

        const retrospectiveModalActive = ref(false);
        const feedbackModalActive = ref(false);

        const getAllRetrospectives = async () => {
            try{
                const data = await apiService.getAllRetrospectives();
                retrospectiveItems.value = data;
            }
            catch(error){
                console.error("Error fetching retrospectives", error)
            }
        }

        const addNewRetrospective = async (retrospective) => {
            
            try{
                await apiService.createRetrospective(retrospective);
                alert("Retrospective created successfully!");  
                showAddRetrospectiveModal(false)              
            }
            catch(error){
                console.error("Error creating retrospective",error);
                alert("An error occurred when creating retrospective")
            }
        }

        const addNewRetrospectiveFeedback = async (feedback) => {
            try{
                await apiService.createRetrospectiveFeedback(feedback)
                alert("Retrospective feedback created successfully!")
                showFeedbackModal(false)
            }
            catch(error){
                console.error("Error creating retrospective feedback", error)
                alert("An error occurred when creating feedback")
            }
        }

        const showAddRetrospectiveModal = (show) => {
            retrospectiveModalActive.value = show;
        }

        const showFeedbackModal = (show) => {
            feedbackModalActive.value = show
        }

        const viewRetrospectiveFeedback = (retrospectiveName, feedbackItems) => {
            selectedFeedbackItems.value = feedbackItems
            selectedRetrospectiveName.value = retrospectiveName
            showFeedbackModal(true)
        }


        getAllRetrospectives()

        return { 
            retrospectiveItems, 
            selectedFeedbackItems,
            selectedRetrospectiveName,
            retrospectiveModalActive,
            feedbackModalActive,
            getAllRetrospectives,
            addNewRetrospective,
            addNewRetrospectiveFeedback,
            showAddRetrospectiveModal,
            showFeedbackModal,
            viewRetrospectiveFeedback,


        }

    }


})
</script>

<style scoped> 
.grid-container{
    padding: 30px 30px 30px 30px;
}



</style>