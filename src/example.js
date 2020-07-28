import React, {useState} from "react";
import ReactDOM from "react-dom";
import PropTypes from 'prop-types';
import {CounterComponent} from "./CounterComponent.fsproj"
import {TodoComponent} from "./TodoComponent/TodoComponent.fsproj"
import reactToWebComponent from "react-to-webcomponent";
// import register from 'preact-custom-element';
// register(CounterComponent, 'counter-component');

// defining props for reactToWebComponent
TodoComponent.propTypes = {
  todo: PropTypes.string
};
const todoComponent = reactToWebComponent(TodoComponent, React, ReactDOM);
customElements.define("todo-component", todoComponent);

// can wrap your component in React JSX/component, etc.
const WrapperComponent = () => {
  const [count, setCount] = useState(100);
  const [show, setShow] = useState(true);

  return <div>
      <p>
        <button onClick={() => setShow(!show)}>Toggle elmish component</button>
        Check the console to see the unmount message
      </p>

      { show && <CounterComponent count={count} setCount={setCount}/> }

    </div>;
}
const counterComponent = reactToWebComponent(WrapperComponent, React, ReactDOM);
customElements.define("counter-component", counterComponent);
