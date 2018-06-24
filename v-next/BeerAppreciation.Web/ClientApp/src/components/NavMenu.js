import React from 'react';
import { Link } from 'react-router-dom';
import { LinkContainer } from 'react-router-bootstrap';

export default props => (
    <nav className="col-md-2 d-none d-md-block bg-light sidebar">
        <div className="sidebar-sticky">
            <ul className="nav flex-column">
                <li className="nav-item">
                <LinkContainer to={'/'} exact>
                    <a className="nav-link">
                        <span data-feather="home"></span>
                        Home
                        <span className="sr-only">(current)</span>
                    </a>
                </LinkContainer>
                </li>
                <li className="nav-item">
                    <LinkContainer to={'/counter'}>
                        <a className="nav-link">
                            <span>Counter</span>
                        </a>
                    </LinkContainer>
                </li>
                <li className="nav-item">
                    <LinkContainer to={'/fetchdata'}>
                        <a className="nav-link">
                            <span>Fetch Data</span>
                        </a>
                    </LinkContainer>
                </li>
            </ul>
        </div>
    </nav>
);

//<Navbar inverse fixedTop fluid collapseOnSelect>
//    <Navbar.Header>
//    <Navbar.Brand>
//    <Link to={'/'}>BeerAppreciation.Web</Link>
//    </Navbar.Brand>
//    <Navbar.Toggle />
//    </Navbar.Header>
//    <Navbar.Collapse>
//    <Nav>
//    <LinkContainer to={'/'} exact>
//    <NavItem>
//    <Glyphicon glyph='home' /> Home
//    </NavItem>
//    </LinkContainer>
//    <LinkContainer to={'/counter'}>
//    <NavItem>
//    <Glyphicon glyph='education' /> Counter
//    </NavItem>
//    </LinkContainer>
//    <LinkContainer to={'/fetchdata'}>
//    <NavItem>
//    <Glyphicon glyph='th-list' /> Fetch data
//    </NavItem>
//    </LinkContainer>
//    </Nav>
//    </Navbar.Collapse>
//    </Navbar>

