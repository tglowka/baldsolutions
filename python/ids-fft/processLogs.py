import matplotlib.pyplot as plt
import pandas
from scipy.fft import fft
import numpy as np

# read log file
data = pandas.read_csv(
    'logs.csv',
    parse_dates=['date'],
    index_col='date',
    usecols=['date', 'count']
)

# time domain
resampledData = data.resample('60S').sum().reset_index()

x = resampledData['date']
y = resampledData['count']

plt.plot(x, y)
plt.xlabel('Datetime (mm-dd hh)')
plt.ylabel('Count')
plt.show()

# frequency domain
resampledData = data.resample('15S').sum().reset_index()

# fourier transform
fft = np.fft.rfft(resampledData['count'])
freq = np.fft.rfftfreq(len(resampledData['count']), 15)

plt.stem(freq, abs(fft))
plt.xlabel('Frequency')
plt.ylabel('Count')
plt.show()
