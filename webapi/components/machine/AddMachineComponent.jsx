import React from 'react';
import Box from '@mui/material/Box';
import TextField from '@mui/material/TextField';
import Button from '@mui/material/Button';
import Modal from '@mui/material/Modal';
import { useState } from 'react';
import Typography from '@mui/material/Typography';
import Grid from '@mui/material/Grid';
import machineService from '../../services/machineService';

function AddMachineComponent({ setAddModal, isAddModalOpen, handleCloseAdd, refreshMachines }) {

    const [machineName, setMachineName] = useState('');
    const [description, setDescription] = useState('');

    const handleSaveClick = async () => {
        try {
            const machine = {
                machineName: machineName,
                description: description,
                messageCount: 0,
                status: 'idle'
            };

            let newMachine = await machineService.addMachine(machine);
            console.log("New machine: ", newMachine);
            refreshMachines();
            handleCloseAdd();
            setAddModal(false);
        } catch (error) {
            console.error("Some error occured: ", error)
        }
    }

    const handleCancelClick = () => {
        setAddModal(false);
    }

    const handleChangeName = (event) => {
        setMachineName(event.target.value);
    }

    const handleChangeDescription = (event) => {
        setDescription(event.target.value);
    }

    const style = {
        position: 'absolute',
        top: '50%',
        left: '50%',
        transform: 'translate(-50%, -50%)',
        width: '15%',
        bgcolor: 'background.paper',
        boxShadow: 24,
        p: 4,
        '& .MuiTextField-root': { m: 1, width: '25ch' }
    };

    return (
        <Modal open={isAddModalOpen} onClose={handleCloseAdd}>
            <Box
                component="form"
                sx={style}
                noValidate
                autoComplete="off"
            >
                <Typography variant="h6" component="h2" gutterBottom>
                    ADD MACHINE
                </Typography>
                <Grid container spacing={2}>
                    <Grid item xs={12}>
                        <TextField
                            required
                            id="machineName"
                            name="machineName"
                            fullWidth
                            label="Machine Name"
                            onChange={handleChangeName}
                            InputLabelProps={{ shrink: true }}
                            value={machineName}
                        />
                    </Grid>
                    <Grid item xs={12}>
                        <TextField
                            required
                            id="description"
                            name="description"
                            fullWidth
                            onChange={handleChangeDescription}
                            label="Description"
                            InputLabelProps={{ shrink: true }}
                            value={description}
                        />
                    </Grid>
                    <Grid item xs={12} sx={{ display: 'flex', justifyContent: 'space-between', mt: 2 }}>
                        <Button variant="contained" color="success" onClick={handleSaveClick}>
                            Save
                        </Button>
                        <Button variant="outlined" color="error" onClick={handleCancelClick}>
                            Cancel
                        </Button>
                    </Grid>
                </Grid>
            </Box>
        </Modal>
    );
};

export default AddMachineComponent;