import React from 'react';
import './App.css';
import 'bootstrap/dist/css/bootstrap.min.css';
import 'react-notifications/lib/notifications.css';
import 'react-bootstrap-typeahead/css/Typeahead.css';
import { Route, Switch, Redirect } from 'react-router-dom';
import { NotificationContainer } from 'react-notifications';
// components
import Header from './components/Header';
import ProjectionDetails from './components/user/ProjectionDetails';
import Projection from './components/user/Projection';
import Dashboard from './components/admin/Dashboard';


function App() {
  return (
    <React.Fragment>
      <Header/>
      <div className="set-overflow-y">
      <Switch>
        <Redirect exact from="/" to="dashboard/Projection" />
        <Route path="/ProjectionDetails/:id" component={ProjectionDetails} />
        <Route path="/Projection" component={Projection} />
        <Route path="/dashboard" component={Dashboard} />
      </Switch>
      <NotificationContainer />
      </div>
    </React.Fragment>
  );
}

export default App;
