import * as React from 'react';
import { useState, useEffect } from 'react';
import Box from '@mui/material/Box';
import Card from '@mui/material/Card';
import CardActions from '@mui/material/CardActions';
import CardContent from '@mui/material/CardContent';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';
//import Modal from "@mui/material/Modal";
import messageService from '../services/messageService';
import machineService from '../services/machineService';
import MessageComponent from './MessageComponent';
import Swal from 'sweetalert2'

function MachineCardComponent(props) {

    const [isModalOpen, setModal] = useState(false);
    const [messages, setMessages] = useState([]);
    const [columns, setColumns] = useState([]);
    const [loading, setLoading] = useState(false);

    const handleOpen = async () => {
        setLoading(true);
        await seeMessages();
        setLoading(false);
        setModal(true);
    };

    const handleClose = () => {
        setModal(false);
    };

    const seeMessages = async () => {
        try {
            const data = await messageService.getMessagesByClientId(props.machine.id);
            setMessages(data.messages);
            setColumns(data.columns);
            console.log(data);
        } catch (error) {
            console.error("Error while fetching the messages and columns!", error);
        }
    };

    const handleRemove = async (id) => {
        const result = await Swal.fire({
            title: "Are you sure?",
            text: "You won't be able to revert this! All the messages from this machine will also be deleted!",
            icon: "warning",
            showCancelButton: true,
            confirmButtonColor: "#3085d6",
            cancelButtonColor: "#d33",
            confirmButtonText: "Yes, delete it!"
        });

        if (result.isConfirmed) {
            try {
                await machineService.deleteMachine(id);
                await props.refreshMachines();
                Swal.fire({
                    title: "Removed!",
                    text: "Machine has been removed.",
                    icon: "success"
                });
            } catch (error) {
                console.error("Something went wrong while removing machine: ", error);
                Swal.fire({
                    title: "Error!",
                    text: "Something went wrong while removing the machine.",
                    icon: "error"
                });
            }
        }
    }

    return (
        <Box sx={{
            minWidth: 275,
            maxWidth: 300,
            margin: '10px',
            transition: 'transform 0.3s, box-shadow 0.3s',
            '&:hover': {
                transform: 'scale(1.05)',
                boxShadow: 6,
            },
        }}
        >
            <Card variant='outlined' sx={{ height: '100%' }}>
                <React.Fragment>
                    <CardContent>
                        <Typography sx={{ fontSize: 14 }} color='text.secondary' gutterBottom>
                            Production Machines
                        </Typography>
                        <Typography variant='h5' component='div'>
                            {props.machine.machineName}
                        </Typography>
                        <Typography sx={{ mb: 1.5 }} color='text.secondary'>
                            {props.machine.description}
                        </Typography>
                        <Typography variant='body2'>
                            {props.machine.status}
                        </Typography>
                        <br></br>
                        <Typography sx={{ mb: 1.5 }} color='text.secondary'>
                            Message Count: {props.machine.messageCount}
                        </Typography>
                    </CardContent>
                    <CardActions>
                        <Button size='small' onClick={() => handleOpen()}> See Messages
                        </ Button>
                        <Button size='small' onClick={() => handleRemove(props.machine.id)}> Remove Machine
                        </ Button>
                    </CardActions>
                </React.Fragment>
            </Card>
            <MessageComponent
                isOpen={isModalOpen}
                handleClose={handleClose}
                messages={messages}
                columns={columns}
                machineId={props.machine.id}
                refreshMachines={props.refreshMachines}
            />
        </Box>
    );
}

export default MachineCardComponent;