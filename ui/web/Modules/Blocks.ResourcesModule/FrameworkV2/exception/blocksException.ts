export default class blocksException  {
    protected exception:any;

    constructor(exception) {
        this.exception = exception;
    }

    get Exception(){
        return this.exception;
    }
    
}