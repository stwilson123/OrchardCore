import $ from "jquery"
class RefElement
{
    constructor(el:any)
    {
        this.$el = $(el);
    }
    $el:any;

    public innerHeight(...args):any
    {
        return args.length > 0 ? this.$el.innerHeight(...args) : this.$el.innerHeight();
    }

    public outerHeight(...args):any
    {
        return args.length > 0 ? this.$el.outerHeight(...args) : this.$el.outerHeight();
    }

    public height(...args):any
    {
        return args.length > 0 ? this.$el.height(...args) : this.$el.height();
    }
}

class NullRefElement extends RefElement
{
    constructor()
    {
        super(null);
    }
    public innerHeight(...args):any
    {
        return 0;
    }

    public outerHeight(...args):any
    {
        return 0;
    }

    public height(...args):any
    {
        return 0;
    }
}

let refNullElement = new NullRefElement();



export { RefElement,refNullElement  }


 