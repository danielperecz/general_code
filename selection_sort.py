def selection_sort(lst):
	i = 0
	while i < len(lst):
		p = i
		j = i + 1
		while j < len(lst):
			if lst[j] < lst[p]:
				p = j
			j += 1
		tmp = lst[p]
		lst[p] = lst[i]
		lst[i] = tmp
		i += 1
	return lst
