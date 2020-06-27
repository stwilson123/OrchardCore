import blocksExpcetion from "../exception/blocksException";
export default class httpException extends blocksExpcetion {
    constructor(exception) {
        super(exception)
    }
}