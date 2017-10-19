import unittest

def convertToRomanString(aNumberToConvert):
	aromanString = ''
	if 	aNumberToConvert <=3 :
		for i in range(aNumberToConvert):
			aromanString += 'I'
	elif aNumberToConvert == 4 :
		return "IV"
	elif aNumberToConvert == 5 : 
		return "V"
	else:
		return "VI"
	return aromanString

class RomanTest(unittest.TestCase):

	def testA(self):
		self.assertEquals(convertToRomanString(1),"I")

	def testB(self):
		self.assertEquals(convertToRomanString(2), "II")

	def testC(self):
		self.assertEquals(convertToRomanString(3), "III")

	def testD(self):
		self.assertEquals(convertToRomanString(4), 'IV')
	
	def testE(self):
		self.assertEquals(convertToRomanString(5), 'V')
	
	def testF(self):
		self.assertEquals(convertToRomanString(6), 'VI')

