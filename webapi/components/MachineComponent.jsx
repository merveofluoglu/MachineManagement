import React, { useState, useEffect } from 'react';
import machineService from '../services/machineService';
import CircularProgress from '@mui/material/CircularProgress';
import Box from '@mui/material/Box';
import MachineCardComponent from './MachineCardComponent';
import NavbarComponent from './NavbarComponent';
import SidebarComponent from './SidebarComponent';

function MachineComponent() {

    const [machines, setMachines] = useState([]);
    const [isSidebarOpen, setSidebarOpen] = useState(false);
    const [loading, setLoading] = useState(false);

    useEffect(() => {
        const fetchMachines = async () => {
            try {
                setLoading(true);
                const machineData = await machineService.getAllMachines();
                setMachines(machineData);
                setLoading(false);
            } catch (error) {
                console.error("Error while fetching the machines!", error);
            }
        };

        fetchMachines();
    }, []);

    const toggleSidebar = () => {
        setSidebarOpen(!isSidebarOpen);
    };

    const refreshMachines = async () => {
        try {
            const result = await machineService.getAllMachines();
            console.log("Machines: ", result);
            setMachines(result);
        } catch (error) {
            console.error("Something went wrong while fetching all machines: ", error);
        }

    }

    return (
        <Box sx={{ display: 'flex', flexDirection: 'column', minHeight: '100vh' }}>
            <NavbarComponent toggleSidebar={toggleSidebar} />
            <SidebarComponent isOpen={isSidebarOpen} toggleSidebar={toggleSidebar} refreshMachines={refreshMachines} />
            {loading ?
                (<Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100vh' }}>
                    <CircularProgress />
                </Box>)
                :
                <Box sx={{ mt: 8, p: 2, flexGrow: 1 }}>
                    <Box sx={{ display: 'flex', flexWrap: 'wrap', gap: 2 }}>
                        {machines.map((machine) => (
                            <MachineCardComponent key={machine.id} machine={machine} refreshMachines={refreshMachines} />
                        ))}
                    </Box>
                </Box>
            }
        </Box>
    );    
}

export default MachineComponent;