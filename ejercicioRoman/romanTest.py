import unittest
def convertToRomanString(aNumberToConvert):
	aromanString = ''

	if aNumberToConvert < 10:
		aromanString = numberLessThan10(aNumberToConvert)
	if  10 <= aNumberToConvert <= 19:
		aromanString = 'X' + numberLessThan10(aNumberToConvert-10)
	if 20 <= aNumberToConvert <= 23:
		aromanString = 'XX'
		aromanString = onesOf(aNumberToConvert - 20, aromanString)
	if aNumberToConvert == 24:
		aromanString = 'XXIV'
	if 25 <= aNumberToConvert <= 28:
		aromanString = 'XXV'
		aromanString = onesOf(aNumberToConvert - 25, aromanString)
	if aNumberToConvert == 29:
		aromanString = 'XXIX'

	return aromanString

def numberLessThan10(aNumberToConvert):
	aromanString = ''

	if aNumberToConvert <= 3:
		aromanString = onesOf(aNumberToConvert, aromanString)
	if aNumberToConvert == 4:
		return "IV"
	if 5 <= aNumberToConvert <= 8:
		aromanString = 'V'
		aromanString = onesOf(aNumberToConvert - 5, aromanString)
	if aNumberToConvert == 9:
		aromanString = 'IX'

	return aromanString

def onesOf(aNumberToConvert, aromanString):
	for i in range(aNumberToConvert):
		aromanString += 'I'
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
		self.assertEquals(convertToRomanString(6), 'VI')
		self.assertEquals(convertToRomanString(7), 'VII')
		self.assertEquals(convertToRomanString(8), 'VIII')

	def testF(self):
		self.assertEquals(convertToRomanString(9), 'IX')

	def testG(self):
		self.assertEquals(convertToRomanString(10), 'X')

	def testH(self):
		self.assertEquals(convertToRomanString(11), 'XI')
		self.assertEquals(convertToRomanString(12), 'XII')
		self.assertEquals(convertToRomanString(13), 'XIII')
	def testI(self):
		self.assertEqual(convertToRomanString(14),"XIV")
	def testJ(self):
		self.assertEqual(convertToRomanString(15), 'XV')
		self.assertEqual(convertToRomanString(16), 'XVI')
		self.assertEqual(convertToRomanString(17), 'XVII')
		self.assertEqual(convertToRomanString(18), 'XVIII')
	def testK(self):
		self.assertEqual(convertToRomanString(19), 'XIX')

	def testL(self):
		self.assertEqual(convertToRomanString(20), 'XX')
		self.assertEqual(convertToRomanString(21), 'XXI')
		self.assertEqual(convertToRomanString(22), 'XXII')
		self.assertEqual(convertToRomanString(23), 'XXIII')


	def testM(self):
		self.assertEqual(convertToRomanString(24), 'XXIV')


	def testN(self):
		self.assertEqual(convertToRomanString(25), 'XXV')
		self.assertEqual(convertToRomanString(26), 'XXVI')
		self.assertEqual(convertToRomanString(27), 'XXVII')
		self.assertEqual(convertToRomanString(28), 'XXVIII')

	def testO(self):
		self.assertEqual(convertToRomanString(29), 'XXIX')

	def testPI(self):
		self.assertEqual(convertToRomanString(30), 'XXX')
		self.assertEqual(convertToRomanString(39), 'XXXIX')
