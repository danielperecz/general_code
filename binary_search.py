def binary_search(a, q):
	low = 0
	if q not in a:
		return "Element not in list."
	high = len(a)
	while low < high:
		mid = (low + high) // 2
		if a[mid] < q:
			low = mid + 1
		else:
			high = mid
	return low
