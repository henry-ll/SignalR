using SignalRHub.Web;

var builder = WebApplication.CreateBuilder(args);
//官方文档地址：https://learn.microsoft.com/zh-cn/aspnet/core/tutorials/signalr?view=aspnetcore-7.0&tabs=visual-studio

//在“解决方案资源管理器”> 中，右键单击项目，然后选择“添加”“客户端库”。
//在“添加客户端库”对话框中：
//为“提供程序”选择“unpkg”
//对于“库”，输入 @microsoft/signalr@latest
//选择“选择特定文件”，展开“dist/browser”文件夹，然后选择 signalr.js 和 signalr.min.js。
//将“目标位置”设置为 wwwroot/js/signalr/
//选择“安装”

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
