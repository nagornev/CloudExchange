export class BlobExtensions{
    static saveFile(name, blob){
        try{
            const url = window.URL.createObjectURL(blob);
            const a = document.createElement('a');
            a.href = url;
            a.download = name;
            document.body.appendChild(a);
            a.click();
            a.remove();
            window.URL.revokeObjectURL(url);
            
            return {
                success: true
            }
        }
        catch(ex) {
            return {
                success: false,
                error:{
                    message: ex.message
                }
            }
        }
    }
}