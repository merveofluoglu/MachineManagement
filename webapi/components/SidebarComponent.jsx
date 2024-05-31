import React from 'react';
import Drawer from '@mui/material/Drawer';
import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';
import ListItemText from '@mui/material/ListItemText';
import { useState } from 'react';
import AddMachineComponent from './machine/AddMachineComponent';
import UpdateMachineComponent from './machine/UpdateMachineComponent';

function SidebarComponent({ isOpen, toggleSidebar, machines, refreshMachines }) {

    const [isAddModalOpen, setAddModal] = useState(false);
    const [isUpdateModalOpen, setUpdateModal] = useState(false);

    const sidebarItems = [
        { text: 'Home', onClick: () => handleHomeClick('Home') },
        { text: 'Machines', onClick: () => handleMachinesClick('Machines') },
        { text: 'Add Machine', onClick: () => handleAddMachineClick('Add Machine') },
        { text: 'Update Machine', onClick: () => handleUpdateMachineClick('Update Machine') },
        { text: 'Logout', onClick: () => handleLogoutClick('Logout') }
    ];

    const handleCloseAdd = () => {
        setAddModal(false);
    };

    const handleHomeClick = () => {

    }

    const handleMachinesClick = () => {

    }

    const handleAddMachineClick = () => {
        setAddModal(true);
    }

    const handleUpdateMachineClick = () => {
        setUpdateModal(true);
    }

    const handleLogoutClick = () => {

    }

    return (
        <div>
            {(isAddModalOpen || isUpdateModalOpen) ? (
                isAddModalOpen ? (
                    <AddMachineComponent
                        setAddModal={setAddModal}
                        isAddModalOpen={isAddModalOpen}
                        handleCloseAdd={handleCloseAdd}
                        machines={machines}
                        refreshMachines={refreshMachines}
                    />
                ) : (
                    <UpdateMachineComponent setUpdateModal={setUpdateModal} />
                )
            ) : (
                <Drawer open={isOpen} onClose={toggleSidebar}>
                    <List>
                        {sidebarItems.map((element) => (
                            <ListItem button key={element.text} onClick={element.onClick}>
                                <ListItemText primary={element.text} />
                            </ListItem>
                        ))}
                    </List>
                </Drawer>
            )}
        </div>
    );
}

export default SidebarComponent;
