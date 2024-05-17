import React, { useState, useEffect } from 'react';
import machineService from '../services/machineService';
import MachineCardComponent from './MachineCardComponent';

function MachineComponent() {

    const [machines, setMachines] = useState([]);

    useEffect(() => {
        const fetchMachines = async () => {
            try {
                const machineData = await machineService.getAllMachines();
                setMachines(machineData);
            } catch (error) {
                console.error("Error while fetching the machines!", error);
            }
        };

        fetchMachines();
    }, []);

    return (
        <div>
            {machines.map((machine) => (
                <MachineCardComponent key={machine.id} machine={machine} />
            ))}
        </div>
    );    
}

export default MachineComponent;