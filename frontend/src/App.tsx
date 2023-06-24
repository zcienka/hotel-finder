import React from 'react';
import {LoginRegister} from "./pages/LoginRegister";
import {Route, Routes} from "react-router-dom";
import HomePage from "./pages/HomePage";

function App() {
    return (
        <Routes>
            <Route path="/" element={<HomePage />} />
            <Route path="/l" element={<LoginRegister/>}/>
        </Routes>
    );
}

export default App;
