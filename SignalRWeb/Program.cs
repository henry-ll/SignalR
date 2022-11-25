using SignalRHub.Web;

var builder = WebApplication.CreateBuilder(args);
//�ٷ��ĵ���ַ��https://learn.microsoft.com/zh-cn/aspnet/core/tutorials/signalr?view=aspnetcore-7.0&tabs=visual-studio

//�ڡ����������Դ��������> �У��Ҽ�������Ŀ��Ȼ��ѡ����ӡ����ͻ��˿⡱��
//�ڡ���ӿͻ��˿⡱�Ի����У�
//Ϊ���ṩ����ѡ��unpkg��
//���ڡ��⡱������ @microsoft/signalr@latest
//ѡ��ѡ���ض��ļ�����չ����dist/browser���ļ��У�Ȼ��ѡ�� signalr.js �� signalr.min.js��
//����Ŀ��λ�á�����Ϊ wwwroot/js/signalr/
//ѡ�񡰰�װ��

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapHub<WebChatHub>("/WebChatHub");
app.Run();
