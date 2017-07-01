class ConfigurationList extends React.Component {
    constructor(props) {
        super(props);
        // this will be obtained either by getting json from the page or by calling some http endpoint
        this.state = {
            configKeys: ['some-key', 'some-other-key']
        };
    }

    render() {
        return (
            <div>
                <ul>
                    {this.state.configKeys.map((configKey) =>
                        <li>{configKey}</li>
                    )}
                </ul>
            </div>
        );
    }
}

// the component still needs to be exported, so it can be rendered some place else
ReactDOM.render(
    <ConfigurationList />,
    document.getElementById('root')
);