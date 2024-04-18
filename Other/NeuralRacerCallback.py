

class NeuralRacerCallback:
    """Abstract class used for controlling your NeuralRacer(C) car."""

    def ready_to_race(self):
        """Called when client is connected to the server and waiting for start of a race."""
        print('Ready to race')

    def race_ended(self):
        """Called after successful finish of the race."""
        print('Race ended')

    def response_for(self, round):
        """
        Called periodically to ask for next step based on current car's position.
        Parameters:
            round - Dict with information about current position of your car - should be the same as training data.
        Return:
            Dict with parameters describing adjustments to your car e.g.: {'acc': 1.0, 'wheel': 0.5}
        """
        return {'acc': 1.0, 'wheel': 0.5}
