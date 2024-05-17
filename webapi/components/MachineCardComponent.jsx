import * as React from 'react';
import { useState, useEffect } from 'react';
import Box from '@mui/material/Box';
import Card from '@mui/material/Card';
import CardActions from '@mui/material/CardActions';
import CardContent from '@mui/material/CardContent';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';
import messagesService from '../services/messageService';

function MachineCardComponent(props) {

    //let isModalOpen = false;
    const [isModalOpen, setModal] = useState(false);

    const seeMessages = async () => {
        try {
            let messages = await messagesService.getMessagesByClientId(props.machine.id);
            console.log(messages);
            //isModalOpen = !isModalOpen;
            setModal(!isModalOpen);
        } catch (error) {
            console.error("Error while fetching the machines!", error);
        }
    }    

    return (
        <Box sx={{
            minWidth: 275,
            maxWidth: 300,
            margin: '0 10px 20px 10px'
        }}>
            <Card variant='outlined' sx={{ height: '100%' }}>
                <React.Fragment>
                    <CardContent>
                        <Typography sx={{ fontSize: 14 }} color='text.secondary' gutterBottom>
                            Production Machines
                        </Typography>
                        <Typography variant='h5' component='div'>
                            { props.machine.machineName }
                        </Typography>
                        <Typography sx={{ mb: 1.5 }} color='text.secondary'>
                            { props.machine.description }
                        </Typography>
                        <Typography variant='body2'>
                            { props.machine.status }
                        </Typography>
                        <br></br>
                        <Typography sx={{ mb: 1.5 }} color='text.secondary'>
                            { props.machine.messageCount }
                        </Typography>
                    </CardContent>
                    <CardActions>
                        <Button size='small' onClick={() => seeMessages()}> See Messages
                        </Button>
                        {
                            isModalOpen && (
                                <div className='modal'>
                                    <p>Custom Modal</p>
                                </div>
                            )
                        }
                    </CardActions>
                </React.Fragment>
            </Card>
        </Box>
    );
}

export default MachineCardComponent;