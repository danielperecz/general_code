def manhattan_distance(start, goal):
	# start and goal are the positions, in the format: start = "1 3824756", goal = "1238 4756"
	xy_coordinates = {
		0 : (1,3),
		1 : (2,3),
		2 : (3,3),
		3 : (1,2),
		4 : (2,2),
		5 : (3,2),
		6 : (1,1),
		7 : (2,1),
		8 : (3,1)
	}
    
	sum_of_distances = 0
    
	i = 0
	while i < len(start):
		if start[i] != goal[i] and start[i] != " ":
			x = abs(xy_coordinates[i][0] - xy_coordinates[goal.index(start[i])][0])
			y = abs(xy_coordinates[i][1] - xy_coordinates[goal.index(start[i])][1])
			sum_of_distances += x + y
		i += 1
    
	return sum_of_distances
	
"""
xy_coordinates dictionary explained: as you can see tile 0 is at coordinates (1,3), tile 1 is at (2,3), etc.

y-axis   +
	 |
      3  +---+---+---+
	 | 0 | 1 | 2 |
      2  +---+---+---+
	 | 3 | 4 | 5 |
      1  +---+---+---+
	 | 6 | 7 | 8 |
      0  +---+---+---+---+ x-axis
	 0   1   2   3
"""
	
