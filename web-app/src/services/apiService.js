import axios from 'axios'

const BASE_URL = "http://localhost:5223/api"

const client = axios.create({
    baseURL: BASE_URL
})

export default{
    async getAllRetrospectives(){
        const response = await client.get(`retrospective/all`)
        return response.data
    },

    async getRetrospectivesByDate(date){
        const response = await client.get(`retrospective`,{
            params:{
                date: date
            }
        })
        return response.data
    },

    async createRetrospective(retrospective){
        const response = await client.post(`retrospective`,retrospective)
        return response.data
    },

    async createRetrospectiveFeedback(feedback){
        const response = await client.post(`retrospective/feedback`,feedback)
        return response.data
    }
}