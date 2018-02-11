# [Neural network with genetic algorithms in Unity3d](https://drasite.com/AI-robot)  

![asdf](https://raw.githubusercontent.com/daniruiz/Cleaning-Robot-AI/master/Images/demo.png)

## Neural network with genetic algorithms in Unity3d
The robot is controlled by a Perceptron like neural network that learns thanks to genetics algorithms such as selection, reproduction and mutation. The network consists of 10 inputs (one for each sensor), a hidden layer of 10 neurons and 2 inputs: turning speed and for the velocity of displacement. For the activation function of the neurons a sigmoidal function is used and the mutation rate is fixed to 1%.
## Fitness function:
At the beginning of the test the map will be segmented in grids of the same size of the robot's. For each cleaned grid a point will be added to the fitness value, but, if the robot repeats a cleaned grid, a point will be subtracted to this value. In this way, the entry into infinite loops is also avoided.
