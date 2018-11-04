class TagContainer extends React.Component {
    constructor(props){
        super(props);
    }

    componentDidMount(){
        var envTagContainer = jQuery('#' + this.props.elementId);

        var chipsOptions = {
            placeholder: this.props.placeholder,
            secondaryPlaceholder: this.props.secondaryPlaceholder
        };

        if(this.props.tags !== undefined && this.props.tags.length > 0){
            chipsOptions.data = this.props.tags.map(function(tag, index){
                return {
                    tag: tag
                };
            });
        }

        envTagContainer.material_chip(chipsOptions);

        envTagContainer.on('chip.add', (e, chip) => {
            this.props.addTag(chip.tag);
        });
            
        envTagContainer.on('chip.delete', (e, chip) => {
            this.props.removeTag(chip.tag);
        });
    }

    componentWillReceiveProps(nextProps){

        if(this.props.tags !== nextProps.tags){
            var envTagContainer = jQuery('#' + this.props.elementId);

            var chipsOptions = {
                placeholder: this.props.placeholder,
                secondaryPlaceholder: this.props.secondaryPlaceholder
            };

            if(nextProps.tags !== undefined && nextProps.tags.length > 0){
                chipsOptions.data = nextProps.tags.map(function(tag, index){
                    return {
                        tag: tag
                    };
                });
            }

            envTagContainer.material_chip(chipsOptions);

            // envTagContainer.on('chip.add', (e, chip) => {
            //     this.props.addTag(chip.tag);
            // });
                
            // envTagContainer.on('chip.delete', (e, chip) => {
            //     this.props.removeTag(chip.tag);
            // });
        }
    }

    render(){
        return (<div id={this.props.elementId}></div>);
    }
}

export default TagContainer;