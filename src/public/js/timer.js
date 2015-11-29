import { StateStreamMixin } from 'rx-react';
import React from 'react';
import Rx from 'rx';

export default React.createClass({
  mixins: [
    StateStreamMixin
  ],

  getStateStream() {
    return Rx.Observable.interval(1000).map(function (interval) {
      return {
        secondsElapsed: (interval + 1)
      };
    }).startWith({secondsElapsed: 0});
  },

  render() {
    const { secondsElapsed } = this.state;
    return <div>Seconds Elapsed: {secondsElapsed}s</div>;
  }
});

