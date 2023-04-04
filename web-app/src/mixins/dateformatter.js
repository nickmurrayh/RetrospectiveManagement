export default{
    methods:{
        formatDate(dateString){
            try{
                const date = new Date(dateString);
                return new Intl.DateTimeFormat('default', {dateStyle: 'long'}).format(date);
            }
            catch(error){
                return null
            }
        }
    }
    
}