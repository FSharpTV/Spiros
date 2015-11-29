import React from 'react';
// read up on how RxJS works here:
// https://xgrommx.github.io/rx-book/
import Rx from 'rx';
import RxReact from 'rx-react';
import Timer from './timer';
import mui from 'material-ui';
import sha1 from 'simple-sha1';

const {
  Styles,
  TextField,
  RaisedButton,
  Paper
} = mui;

const {
  FuncSubject,
  StateStreamMixin
} = RxReact;

const _ = require('./underscore.rx')(require('./underscore'));



////////////////////////
// boilerplate below: //
////////////////////////

const App = (function() {
  let injectTapEventPlugin = require("react-tap-event-plugin");
  injectTapEventPlugin();

  let ThemeManager = new Styles.ThemeManager();
  ThemeManager.setTheme(ThemeManager.types.DARK);
  require('../css/index.styl');

  return React.createClass({
    childContextTypes: {
      muiTheme: React.PropTypes.object
    },
    getChildContext() {
      return {
        muiTheme: ThemeManager.getCurrentTheme()
      };
    },
    render() {
      return <ChatApp />;
    }
  });
})();

document.addEventListener("DOMContentLoaded", evt => {
  React.render(<App />, document.body);
});