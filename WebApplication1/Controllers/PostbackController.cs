using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using WebApplication1.Data;
using WebApplication1.Models;
<<<<<<< HEAD
=======
using System.Data;
>>>>>>> 19e1c65 (18.11.2026)

//GET / POST Ayrı - seçkiliyi və Form/Query Birləşdirməsi
// DDoS Qoruması və Stream Buffering (64KB limit)
// Casino IP Whitelisting Təhlükəsizliyi
// Idempotency və Race Condition Qoruması (DB Unique Index + Catch)
// Maliyyə State Machine (reg, ftd, ftd_cancel)
// Tam Audit Tarixçəsi (Həm uğurlu, həm də uğursuz loglar)


[Route("api/postback")]
[ApiController]
public class PostbackController : ControllerBase
{
    private readonly AppDbContext _context;
<<<<<<< HEAD
    private readonly ILogger<PostbackController> _logger;

    public PostbackController(AppDbContext context, ILogger<PostbackController> logger)
    {
        _context = context;
        _logger = logger;
=======
    private readonly IDbContextFactory<AppDbContext> _contextFactory;
    private readonly ILogger<PostbackController> _logger;

    public PostbackController(
        AppDbContext context,
        IDbContextFactory<AppDbContext> contextFactory, 
        ILogger<PostbackController> logger)
    {
        _context = context;
        _logger = logger;
        _contextFactory = contextFactory;
>>>>>>> 19e1c65 (18.11.2026)
    }
    [HttpGet("{casinoSlug}")]
    public async Task<IActionResult> HandleGetPostback(
        string casinoSlug,
        [FromQuery] string? token,
        [FromQuery] string? type,
        [FromQuery] string? payout,
        [FromQuery] string? txId = null)
    {
        string normalizedSlug = casinoSlug?.ToLowerInvariant()?.Trim() ?? string.Empty;
        var rawData = HttpContext.Request.QueryString.Value ?? string.Empty;


        return await ProcessPostbackAsync(normalizedSlug, token, type, payout, txId, rawData);
    }
    [HttpPost("{casinoSlug}")]
    [RequestSizeLimit(1024 * 64)]
    public async Task<IActionResult> HandlePostpostback(
    string casinoSlug,
<<<<<<< HEAD
    [FromQuery] string? qToken,
    [FromQuery] string? qType,
    [FromQuery] string? qPayout,
    [FromForm] string? fPayout,
    [FromQuery] string? qTxId = null,
    [FromForm] string? fToken = null,
    [FromForm] string? fType = null,
    [FromForm] string? fTxId = null)
    {
        // 1. Prioritet olaraq Formadan gələni götürürük, yoxdursa Query-dən gələni
        var token = fToken ?? qToken;
        var type = fType ?? qType;
        var payout = fPayout ?? qPayout; // 👈 Artıq təmiz string olaraq seçildi!
        var txId = fTxId ?? qTxId;
=======
    [FromQuery(Name = "token")] string? qToken,
    [FromQuery(Name = "type")] string? qType,
    [FromQuery(Name = "payout")] string? qPayout,
    [FromQuery(Name = "txId")] string? qTxId,

    [FromForm(Name = "token")] string? fToken,
    [FromForm(Name = "type")] string? fType,
    [FromForm(Name = "payout")] string? fPayout,
    [FromForm(Name = "txId")] string? fTxId)
    {
        // 1. Prioritet olaraq Formadan gələni götürürük, yoxdursa Query-dən gələni
        var token = !string.IsNullOrWhiteSpace(fToken) ? fToken : qToken;
        var type = !string.IsNullOrWhiteSpace(fType) ? fType : qType;
        var payout = !string.IsNullOrWhiteSpace(fPayout) ? fPayout : qPayout;
        var txId = !string.IsNullOrWhiteSpace(fTxId) ? fTxId : qTxId;

>>>>>>> 19e1c65 (18.11.2026)

        // 2. RawData-nı oxuyuruq
        HttpContext.Request.EnableBuffering();
        using var reader = new StreamReader(HttpContext.Request.Body, leaveOpen: true);
        var bodyString = await reader.ReadToEndAsync();
        HttpContext.Request.Body.Position = 0;

        string normalizedSlug = casinoSlug?.ToLowerInvariant()?.Trim() ?? string.Empty;
        var rawData = $"Query: {HttpContext.Request.QueryString} | Body: {bodyString}";

        // 3. ProcessPostbackAsync metoduna string olaraq ötürürük
        return await ProcessPostbackAsync(normalizedSlug, token, type, payout, txId, rawData);
    }
    private async Task<IActionResult> ProcessPostbackAsync(
        string casinoSlug,string? token, string? type,
        string? payout, string? txId, string rawData)
    {
        //İlkin tənzimləmələr
        var clientIp = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault()?.Split(',')[0]?.Trim()
                       ?? HttpContext.Connection.RemoteIpAddress?.ToString()
                       ?? "Unknown";

        string normalizedType = type?.ToLowerInvariant()?.Trim() ?? string.Empty;
        string normalizedSlug = casinoSlug?.ToLowerInvariant()?.Trim() ?? string.Empty;
        string cleanTxId = txId?.Trim() ?? string.Empty;

        if (!string.IsNullOrEmpty(rawData) && rawData.Length > 2000)
        {
            rawData = rawData.Substring(0, 2000);
        }

        // 1. Kazino yoxlaması
        var casino = await _context.Casinos.AsNoTracking().FirstOrDefaultAsync(c => c.Slug == normalizedSlug);
        if (casino == null)
        {
            await SaveFailedPostbackAsync(
                 casinoId: null,                            
                 clickLogId: null,                          
                 type: normalizedType,                      
                 payout: 0,                                 
                 rawData: rawData,                         
                 ipAddress: clientIp,                      
                 txId: cleanTxId,                          
                 errorMessage: $"Invalid casino slug: {normalizedSlug}");
            _logger.LogWarning("Naməlum kazinodan postback cəhdi. Slug: {CasinoSlug}, IP: {ClientIp}", casinoSlug, clientIp);
            return Ok(new { status = "error", message = "Casino not found" });
        }

        // 2. Payout formatı və mənfi olub-olmaması yoxlaması
<<<<<<< HEAD
        if (!decimal.TryParse(payout, out decimal parsedPayout) || parsedPayout < 0)
=======

        if (!decimal.TryParse(
            payout,
            NumberStyles.AllowDecimalPoint,
            CultureInfo.InvariantCulture,
            out decimal parsedPayout)
        || parsedPayout < 0)
>>>>>>> 19e1c65 (18.11.2026)
        {
            await SaveFailedPostbackAsync(
                 casinoId: casino.Id,
                 clickLogId: null,
                 type: normalizedType,
                 payout: 0,
                 rawData: rawData,
                 ipAddress: clientIp,
                 txId: cleanTxId,
                 errorMessage: $"Invalid or negative payout format: {payout}");
            return BadRequest(new { status = "error", message = "Invalid or negative payout format" });
        }

        // 3. IP Whitelisting yoxlaması
        if (!string.IsNullOrEmpty(casino.AllowedIPs))
        {
            var allowedIps = casino.AllowedIPs.Split(',').Select(ip => ip.Trim()).ToList();
<<<<<<< HEAD
            if (!allowedIps.Contains(clientIp) && clientIp != "Unknown")
            {
                _logger.LogWarning("Kazino IP uyğunsuzluğu! Casino: {CasinoSlug}, Gələn IP: {ClientIp}", casinoSlug, clientIp);
                await SaveFailedPostbackAsync(
                   casinoId: casino.Id,
                   clickLogId: null,
                   type: normalizedType,
                   payout: parsedPayout,
                   rawData: rawData,
                   ipAddress: clientIp,
                   txId: cleanTxId,
                   errorMessage: $"IP not allowed: {clientIp}");
                return StatusCode(403, new { status = "error", message = "IP not allowed" });
=======
           
        if (clientIp == "Unknown" ||
                !allowedIps.Contains(clientIp))
            {
                await SaveFailedPostbackAsync(
                    casino.Id,
                    null,
                    normalizedType,
                    parsedPayout,
                    rawData,
                    clientIp,
                    cleanTxId,
                    $"IP not allowed: {clientIp}");

                return StatusCode(403, new
                {
                    status = "error",
                    message = "IP not allowed"
                });
>>>>>>> 19e1c65 (18.11.2026)
            }
        }

        // 3. Idempotency yoxlaması
<<<<<<< HEAD
        if (!string.IsNullOrEmpty(cleanTxId))
        {
            bool alreadyProcessed = await _context.PostbackLogs
                .AsNoTracking()
                .AnyAsync(p => p.TransactionId == cleanTxId && p.CasinoId == casino.Id && p.IsSuccessful);

            if (alreadyProcessed)
            {
                return Ok(new { status = "success", message = "Duplicate postback ignored by TxId" });
            }
        }
        else
        {
            await SaveFailedPostbackAsync(
                 casinoId: casino.Id,
                 clickLogId: null,
                 type: normalizedType,
                 payout: parsedPayout,
                 rawData: rawData,
                 ipAddress: clientIp,
                 txId: cleanTxId,
                 errorMessage: "Transaction ID (txId) is required");
            return BadRequest(new { status = "error", message = "Transaction ID (txId) is required" });
        }

=======
        if (string.IsNullOrEmpty(cleanTxId))
        {
            await SaveFailedPostbackAsync(
                casinoId: casino.Id,
                clickLogId: null,
                type: normalizedType,
                payout: parsedPayout,
                rawData: rawData,
                ipAddress: clientIp,
                txId: cleanTxId,
                errorMessage: "Transaction ID (txId) is required");

            return BadRequest(new
            {
                status = "error",
                message = "Transaction ID (txId) is required"
            });
        }
>>>>>>> 19e1c65 (18.11.2026)

        // Token format yoxlaması
        if (!Guid.TryParse(token, out var parsedToken))
        {
            await SaveFailedPostbackAsync(
                 casinoId: casino.Id,
                 clickLogId: null,
                 type: normalizedType,
                 payout: parsedPayout,
                 rawData: rawData,
                 ipAddress: clientIp,
                 txId: cleanTxId,
                 errorMessage: "Invalid token format"); 
            return Ok(new { status = "error", message = "Invalid token format" });
        }

        ClickLog? clickLog = await _context.ClickLogs
                .FirstOrDefaultAsync(c => c.Token == parsedToken && c.CasinoId == casino.Id);

        if (clickLog == null)
        {
            await SaveFailedPostbackAsync(
                 casinoId: casino.Id,
                 clickLogId: null,
                 type: normalizedType,
                 payout: parsedPayout,
                 rawData: rawData,
                 ipAddress: clientIp,
                 txId: cleanTxId,
                 errorMessage: "Click token not found");
            return Ok(new { status = "error", message = "Click token not found" });
        }

        // ⏱️ 1. Time Validation (Tracking Window Yoxlaması)
        const int maxTrackingDays = 30;
        if (clickLog.ClickTime < DateTime.UtcNow.AddDays(-maxTrackingDays))
        {
            await SaveFailedPostbackAsync(
                casinoId: casino.Id,
                clickLogId: clickLog.Id,
                type: normalizedType,
                payout: parsedPayout,
                rawData: rawData,
                ipAddress: clientIp,
                txId: cleanTxId,
                errorMessage: "Click expired (tracking window exceeded)");
            return BadRequest(new { status = "error", message = "Tracking window has expired for this click" });
        }
<<<<<<< HEAD
        // 4. Tranzaksiya bloku
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {

=======
        var allowedTypes = new[] { "reg", "ftd", "ftd_cancel" };

        if (!allowedTypes.Contains(normalizedType))
        {
            await SaveFailedPostbackAsync(
                casinoId: casino.Id,
                clickLogId: clickLog.Id,
                type: normalizedType,
                payout: parsedPayout,
                rawData: rawData,
                ipAddress: clientIp,
                txId: cleanTxId,
                errorMessage: $"Unknown postback type: {normalizedType}"
            );

            return BadRequest(new
            {
                status = "error",
                message = "Unknown postback type"
            });
        }
        var existingPostback = await _context.PostbackLogs
    .AsNoTracking()
    .FirstOrDefaultAsync(p =>
        p.CasinoId == casino.Id &&
        p.TransactionId == cleanTxId &&
        p.PostbackType == normalizedType &&
        p.IsSuccessful);

        if (existingPostback != null)
        {
            if (IsSamePostbackPayload(
                existingPostback,
                clickLog.Id,
                normalizedType,
                parsedPayout))
            {
                return BuildDuplicatePostbackResponse(existingPostback);
            }

            await LogPostbackConflictAsync(
                existingPostback,
                clickLog.Id,
                normalizedType,
                parsedPayout,
                rawData,
                clientIp,
                cleanTxId);

            return Conflict(new
            {
                status = "error",
                duplicate = true,
                conflict = true,
                message = "TransactionId was already used with a different payload"
            });
        }
        // 4. Tranzaksiya bloku
        using var transaction = await _context.Database.BeginTransactionAsync(
            IsolationLevel.Serializable);

        try
        {
            var existingPostbackInTransaction = await _context.PostbackLogs
    .AsNoTracking()
    .FirstOrDefaultAsync(p =>
        p.CasinoId == casino.Id &&
        p.TransactionId == cleanTxId &&
        p.PostbackType == normalizedType &&
        p.IsSuccessful);

            if (existingPostbackInTransaction != null)
            {
                await transaction.RollbackAsync();

                if (IsSamePostbackPayload(
                    existingPostbackInTransaction,
                    clickLog.Id,
                    normalizedType,
                    parsedPayout))
                {
                    return BuildDuplicatePostbackResponse(
                        existingPostbackInTransaction);
                }

                await LogPostbackConflictAsync(
                    existingPostbackInTransaction,
                    clickLog.Id,
                    normalizedType,
                    parsedPayout,
                    rawData,
                    clientIp,
                    cleanTxId);

                return Conflict(new
                {
                    status = "error",
                    duplicate = true,
                    conflict = true,
                    message = "TransactionId was already used with a different payload"
                });
            }
            PostbackLog? ftdToReverse = null;
>>>>>>> 19e1c65 (18.11.2026)
            // State Machine & Guards
            switch (normalizedType)
            {
                case "reg":
                    if (!clickLog.IsDeposited && !clickLog.IsRegistered)
                    {
                        clickLog.IsRegistered = true;
                        clickLog.Payout = parsedPayout;
                    }
                    break;
                case "ftd":
<<<<<<< HEAD
                    if (!clickLog.IsDeposited)
                    {
                        clickLog.IsRegistered = true;
                        clickLog.IsDeposited = true;
                        clickLog.Payout = parsedPayout;
                    }
                    break;
                case "ftd_cancel":
                    if (clickLog.IsDeposited)
                    {
                        clickLog.IsDeposited = false;
                        clickLog.Payout = 0.0m;
                    }
                    break;
                default:
                    _logger.LogWarning("Naməlum postback tipi. Type: {Type}, Casino: {CasinoSlug}", normalizedType, normalizedSlug);
                    await SaveFailedPostbackAsync(
                           casinoId: casino.Id,
                           clickLogId: clickLog.Id,
                           type: normalizedType,
                           payout: parsedPayout,
                           rawData: rawData,
                           ipAddress: clientIp,
                           txId: cleanTxId,
                           errorMessage: $"Naməlum postback tipi {normalizedType}");
                    break;
=======
                    if (clickLog.IsDeposited)
                    {
                        await transaction.RollbackAsync();

                        await SaveFailedPostbackAsync(
                            casinoId: casino.Id,
                            clickLogId: clickLog.Id,
                            type: normalizedType,
                            payout: parsedPayout,
                            rawData: rawData,
                            ipAddress: clientIp,
                            txId: cleanTxId,
                            errorMessage: "ClickLog already has an active FTD");

                        return Conflict(new
                        {
                            status = "error",
                            duplicate = false,
                            message = "ClickLog already has an active FTD"
                        });
                    }

                    clickLog.IsRegistered = true;
                    clickLog.IsDeposited = true;
                    clickLog.Payout = parsedPayout;
                    break;
                case "ftd_cancel":
                    ftdToReverse = await _context.PostbackLogs
                        .Where(p =>
                            p.ClickLogId == clickLog.Id &&
                            p.CasinoId == casino.Id &&
                            p.TransactionId == cleanTxId &&
                            p.PostbackType == "ftd" &&
                            p.IsSuccessful &&
                            !_context.PostbackLogs.Any(c =>
                                c.IsSuccessful &&
                                c.ReversedPostbackLogId == p.Id))
                        .OrderByDescending(p => p.ProcessedTime)
                        .FirstOrDefaultAsync();

                    if (ftdToReverse == null)
                    {
                        await transaction.RollbackAsync();

                        await SaveFailedPostbackAsync(
                            casinoId: casino.Id,
                            clickLogId: clickLog.Id,
                            type: normalizedType,
                            payout: parsedPayout,
                            rawData: rawData,
                            ipAddress: clientIp,
                            txId: cleanTxId,
                            errorMessage: "No active successful FTD found to cancel");

                        return Conflict(new
                        {
                            status = "error",
                            message = "No active successful FTD found to cancel"
                        });
                    }

                    clickLog.IsDeposited = false;
                    clickLog.Payout = 0m;

                    break;
                default:
                    throw new InvalidOperationException(
                           $"Unexpected postback type: {normalizedType}"
                       );
>>>>>>> 19e1c65 (18.11.2026)
            }

            var postbackLog = new PostbackLog
            {
                ClickLogId = clickLog.Id,
                CasinoId = casino.Id,
                PostbackType = normalizedType,
<<<<<<< HEAD
                Payout = parsedPayout,
=======

                Payout = normalizedType == "ftd_cancel"
        ? ftdToReverse!.Payout
        : parsedPayout,

                ReversedPostbackLogId = normalizedType == "ftd_cancel"
        ? ftdToReverse!.Id
        : null,

>>>>>>> 19e1c65 (18.11.2026)
                RawData = rawData ?? string.Empty,
                IpAddress = clientIp,
                TransactionId = cleanTxId,
                IsSuccessful = true,
                ResponseMessage = "Successfully",
                ProcessedTime = DateTime.UtcNow
            };

            _context.PostbackLogs.Add(postbackLog);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return Ok(new { status = "success", message = "Processed successfully" });
        }
<<<<<<< HEAD
        catch (DbUpdateException dbEx) when (IsDuplicateKeyException(dbEx))
        {
            await transaction.RollbackAsync();
            _logger.LogWarning("Race condition zamanı dublikat postback bloklandı. TxId: {TxId}", txId);
            return Ok(new { status = "success", message = "Duplicate postback caught by DB constraint" });
        }
=======

        catch (DbUpdateConcurrencyException ex)
        {
            await transaction.RollbackAsync();

            _logger.LogWarning(
                ex,
                "ClickLog concurrency conflict. Casino: {CasinoId}, TxId: {TxId}",
                casino.Id,
                cleanTxId
            );

            return Conflict(new
            {
                status = "error",
                message = "ClickLog was modified by another request"
            });
        }

        catch (DbUpdateException dbEx) when (IsDuplicateKeyException(dbEx))
                {
                    await transaction.RollbackAsync();

                    var existingPostbackAfterError = await FindSuccessfulPostbackAsync(
                        casino.Id,
                        cleanTxId,
                        normalizedType);

            if (existingPostbackAfterError != null)
                    {
                        _logger.LogWarning(
                            "Duplicate postback detected. Casino: {CasinoId}, TxId: {TxId}",
                            casino.Id,
                            cleanTxId);

                        return BuildDuplicatePostbackResponse(existingPostbackAfterError);
                    }

                    _logger.LogError(
                        dbEx,
                        "Unique constraint violation, but previous successful postback was not found. Casino: {CasinoId}, TxId: {TxId}",
                        casino.Id,
                        cleanTxId);

                    return Conflict(new
                    {
                        status = "error",
                        message = "Duplicate constraint violation, but original transaction was not found"
                    });
                }
>>>>>>> 19e1c65 (18.11.2026)
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Postback emal edilərkən xəta baş verdi. Token: {Token}", token);

            // İndi clickLog dəyişəni try səviyyəsində elan olunduğu üçün catch-də əlçatandır
            await SaveFailedPostbackAsync(
                           casinoId: casino.Id,
                           clickLogId: clickLog.Id,
                           type: normalizedType,
                           payout: parsedPayout,
                           rawData: rawData,
                           ipAddress: clientIp,
                           txId: cleanTxId,
                           errorMessage: "Internal processing error");
            return Ok(new { status = "error", message = "Internal error handled" });
        }
    }
    //Köməkçi metodlar
<<<<<<< HEAD
=======

>>>>>>> 19e1c65 (18.11.2026)
    private bool IsDuplicateKeyException(DbUpdateException ex)
    {
        if (ex.InnerException is SqlException sqlEx)
        {
<<<<<<< HEAD
            return sqlEx.Number == 2627 || sqlEx.Number == 2601;
        }

        return ex.InnerException?.Message.Contains("UNIQUE") == true ||
               ex.InnerException?.Message.Contains("IX_") == true;
    }
    private async Task SaveFailedPostbackAsync(
        int? casinoId, long? clickLogId, string type, decimal payout,
        string rawData, string ipAddress, string? txId, string errorMessage)
    {
        try
        {
=======
            bool isDuplicateError =
                sqlEx.Number == 2601 ||
                sqlEx.Number == 2627;

            bool isPostbackIndex =
                sqlEx.Message.Contains(
                    "IX_PostbackLogs_CasinoId_TransactionId_PostbackType",
                    StringComparison.OrdinalIgnoreCase
                );

            return isDuplicateError && isPostbackIndex;
        }

        return false;
    }

    private async Task SaveFailedPostbackAsync(
        int? casinoId,
        long? clickLogId,
        string type,
        decimal payout,
        string rawData,
        string ipAddress,
        string? txId,
        string errorMessage)
    {
        try
        {
            await using var context =
                await _contextFactory.CreateDbContextAsync();

>>>>>>> 19e1c65 (18.11.2026)
            var log = new PostbackLog
            {
                CasinoId = casinoId,
                ClickLogId = clickLogId,
                PostbackType = type,
                Payout = payout,
                RawData = rawData,
                IpAddress = ipAddress,
                TransactionId = txId,
                IsSuccessful = false,
                ResponseMessage = errorMessage,
                ProcessedTime = DateTime.UtcNow
            };

<<<<<<< HEAD
            _context.PostbackLogs.Add(log);
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Uğursuz postback logunu bazaya yazarkən xəta baş verdi.");
        }
    }
=======
            context.PostbackLogs.Add(log);

            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed postback audit log could not be saved");
        }
    }

    private IActionResult BuildDuplicatePostbackResponse(
        PostbackLog existing)
    {
        return Ok(new
        {
            status = "success",
            duplicate = true,
            message = "Transaction was already processed",

            originalOperation = new
            {
                clickLogId = existing.ClickLogId,
                postbackType = existing.PostbackType,
                payout = existing.Payout,
                processedTime = existing.ProcessedTime,
                responseMessage = existing.ResponseMessage
            }
        });
    }

    private async Task<PostbackLog?> FindSuccessfulPostbackAsync(
    int casinoId,
    string transactionId,
    string type)
    {
        await using var context =
            await _contextFactory.CreateDbContextAsync();

        return await context.PostbackLogs
            .AsNoTracking()
            .FirstOrDefaultAsync(p =>
                p.CasinoId == casinoId &&
                p.TransactionId == transactionId &&
                p.PostbackType == type &&
                p.IsSuccessful);
    }

    private bool IsSamePostbackPayload(
    PostbackLog existing,
    long incomingClickLogId,
    string incomingType,
    decimal incomingPayout)
    {
        return existing.ClickLogId == incomingClickLogId
            && string.Equals(
                existing.PostbackType,
                incomingType,
                StringComparison.OrdinalIgnoreCase)
            && existing.Payout == incomingPayout;
    }
    private async Task LogPostbackConflictAsync(
    PostbackLog existing,
    long incomingClickLogId,
    string incomingType,
    decimal incomingPayout,
    string rawData,
    string clientIp,
    string transactionId)
    {
        string conflictMessage =
            $"TransactionId conflict. " +
            $"Original: ClickLogId={existing.ClickLogId}, " +
            $"Type={existing.PostbackType}, Payout={existing.Payout}. " +
            $"Incoming: ClickLogId={incomingClickLogId}, " +
            $"Type={incomingType}, Payout={incomingPayout}.";

        await SaveFailedPostbackAsync(
            casinoId: existing.CasinoId,
            clickLogId: incomingClickLogId,
            type: incomingType,
            payout: incomingPayout,
            rawData: rawData,
            ipAddress: clientIp,
            txId: transactionId,
            errorMessage: conflictMessage);

        _logger.LogWarning(
            "Postback transaction conflict. CasinoId: {CasinoId}, TxId: {TxId}",
            existing.CasinoId,
            transactionId);
    }
>>>>>>> 19e1c65 (18.11.2026)
}