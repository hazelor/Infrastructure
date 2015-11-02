using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Hazelor.Infrastructure.Tools
{
    public class PreciseTimer
    {
        static readonly long frequency;
        static readonly EventArgs eventArgs = new EventArgs();

        [DllImport("kernel32.dll")]
        static extern bool QueryPerformanceCounter(out long value);
        [DllImport("kernel32.dll")]
        static extern bool QueryPerformanceFrequency(out long value);

        bool running;
        long rawTicks;
        long interval;
        long timestamp;
        long tickStarted;
        long ticks;
        public int delay;

        // Windows 8，Windows 7 和 Windows Vista 的时钟频率，这是DateTime.Ticks，
        // Environment.TickCount 以及 WinAPI GetSystemTime 的标准频率，但 Task.Delay，
        // Thread.Sleep 或 WinAPI Sleep 的标准频率通常是 15.4 ～ 15.6 毫秒，有时也会超出这个范围
        // 如果 OS 能够提供保证小于等于而不是大于等于所需延迟时间的API，就可以同时保证效率和精确度了
        // 其实将 clockRate 设成 15 毫秒就非常精确了，并不需要一定设成 15.6001674109375
        const double clockRate = 15.6001674109375;


        public event EventHandler<EventArgs> Elapsed;

        static PreciseTimer()
        {
            // 获取频率 (每秒的 Tick 数量)
            QueryPerformanceFrequency(out frequency);
        }

        public PreciseTimer()
        {
        }

        public PreciseTimer(double interval)
        {
            Interval = interval;
        }

        // 获取或设置毫秒间隔 (支持小数，最小值为 clockRate)
        public double Interval
        {
            get
            {
                return interval * 1000.0 / frequency;
            }
            set
            {
                interval = (long)Math.Round(value * frequency / 1000);

                // 如果 clockRate 的数值过高，也就是 delay 的数值过低，可能会增加 CPU 的使用
                // 如果 delay == 0 会导致 100% 的 CPU 使用，但是如果 delay 不是 0，
                // 则 Task.Dealy 的实际间隔将近似于 clockRate 的倍数
                delay = (int)Math.Ceiling(clockRate < value ? value - clockRate : value);

                // 下面的优化算法竟然不够精确，为什么？
                // delay = (int)Math.Round(2 * value - clockRate
                // * Math.Ceiling(value / clockRate)) - 1;
            }
        }

        // 获取每秒的 Tick 数量
        public static long TicksPerSecond
        {
            get { return frequency; }
        }

        public long Ticks
        {
            get
            {
                return running ? ticks + RawTicks - tickStarted : ticks;
            }
        }

        // 获取 RAW 时间戳 (Tick的数量)
        public long RawTicks
        {
            get
            {
                QueryPerformanceCounter(out rawTicks);
                return rawTicks;
            }
        }

        // 获取所用的毫秒数 (支持小数)
        public double Milliseconds
        {
            get
            {
                return Ticks * 1000.0 / frequency;
            }
        }

        // 获取所用的秒数 (支持小数)
        public double Seconds
        {
            get
            {
                return (double)Ticks / frequency;
            }
        }

        // 启动 PreciseTimer
        public async void Start()
        {
            if (running)
                return;
            running = true;
            Reset();
            timestamp = RawTicks + interval;
            while (running)
            {
                // 如果把下面的 timestamp = RawTicks + interval 放在此行，精确度会降低
                await Task.Delay(delay);
                while (RawTicks < timestamp)
                    // 其实连续调用 Task.Delay(0) 会比 Thread.Sleep(0) 和 Thread.Yield()
                    // 花费的资源更多一些，只是 Thread.Sleep(0) 和 Thread.Yield() 都不是
                    //「awaitable」 的
                    await Task.Delay(0);
                timestamp = RawTicks + interval;
                if (running)
                {
                    Elapsed(this, eventArgs);

                }
            }
        }

        // 停止 PreciseTimer
        public void Stop()
        {
            ticks += RawTicks - tickStarted;
            if (running)
                running = false;
        }

        // 重置 PreciseTimer
        public void Reset()
        {
            ticks = 0;
            tickStarted = RawTicks;
        }

        public override string ToString()
        {
            // 由于精确度的原因，这里没有使用 DateTime 或 TimeSpan 构建字符串
            var t = (double)Ticks / frequency;
            return new StringBuilder(19).Append((int)(t / 3600)).Append(':')
            .Append((int)((t % 3600) / 60)).Append(':').Append(t % 60).ToString();
        }

    }
}
