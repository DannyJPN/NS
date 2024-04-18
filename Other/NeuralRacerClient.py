import socket as sock
import NeuralRacerCallback as cb


class NeuralRacerClient:
    """
    Class for connecting to the NeuralRacer server

    Example:
        nrc = NeuralRacerClient('localhost', 9461)
        l_races = nrc.list_races()
        l_cars = nrc.list_cars(l_races[0])

        callback = ExampleNeuronRacerCallback()
        nrc.join(l_races[0], l_cars[0], 'The Stig', callback)
    """

    def __init__(self, ip, port):
        """
        Parameters:
            ip - Address of the NeuralRacer server.
            port - Port of the NeuralRacer server.
        """
        self._ip = ip
        self._port = port

    def _send(self, cmds, socket):
        for cmd in cmds:
            socket.send((cmd + '\n').encode())
        socket.send('\n'.encode())

    def _read(self, socket, validate=True):
        response = socket.recv(1024).decode()
        response = [r for r in response.split('\n') if r != '']
        if not validate:
            return response
        if response[0] == 'ok':
            return response[1:]
        else:
            raise ValueError(response)

    def _connected_socket(self):
        socket = sock.socket()
        socket.connect((self._ip, self._port))
        return socket

    def _simple_query(self, cmds):
        with self._connected_socket() as socket:
            self._send(cmds, socket)
            return self._read(socket)

    def _race(self, socket, callback):
        callback.ready_to_race()
        while True:
            response = self._read(socket, validate=False)
            if len(response) is 0:
                continue
                # raise ValueError('Empty response - Always make sure you are sending valid race parameters.')
            elif response[0] == 'round':
                round = response[1:]
                round = [param.split(':') for param in round]
                round = dict((param[0], float(param[1])) for param in round)
                cb_response = callback.response_for(round)
                cb_response = [key+':'+str(val) for key, val in zip(cb_response.keys(), cb_response.values())]
                cb_response = ['ok'] + cb_response
                self._send(cb_response, socket)

            elif response[0] == 'finish':
                callback.race_ended()
                return
            else:
                raise ValueError(response)

    def list_races(self):
        """
        Return:
            List of races currently available on the server
        """
        return self._simple_query(['racelist'])

    def list_cars(self, race_name):
        """
        Return:
            List of cars currently available on the server
        """
        return self._simple_query(['carlist', 'race:' + race_name])

    def join(self, race_name, car_name, driver_name, callback=cb.NeuralRacerCallback()):
        """
        Join the race on the server.
        Parameters:
            race_name - Valid race name.
            car_name - Valid car name.
            driver_name - Name to be displayed during the race.
            callback - Instance of a class inheriting from NeuralRacerCallback.
        """
        with self._connected_socket() as socket:
            cmds = [
                'driver',
                'race:'+race_name,
                'driver:'+driver_name,
                'color:0000FF',
                'car:'+car_name
            ]
            # Send commands
            self._send(cmds, socket)
            _ = self._read(socket, validate=False)
            # Start racing!
            self._race(socket, callback)
