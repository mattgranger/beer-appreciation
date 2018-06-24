import React from 'react';
import Header from './LayoutHeader';
import NavMenu from './NavMenu';

export default props => (
    <div>
        <Header />
        <div className="container-fluid">
            <div className="row">
                <NavMenu />
                <main role="main" className="col-md-9 ml-sm-auto col-lg-10 pt-3 px-4">
                    <div className="d-flex justify-content-between flex-wrap flex-md-nowrap align-items-center pb-2 mb-3 border-bottom">
                        {props.children}
                    </div>
                </main>
            </div>
        </div>
    </div>
);
